using System.Windows;
using Caleb_Marks_Assignment_10._3.Models;
using Caleb_Marks_Assignment_10._3.Services;

namespace Caleb_Marks_Assignment_10._3
{
    public partial class MainWindow : Window
    {
        CRUD crud = new CRUD();

        // Build window & attach Loaded event
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        // Show cars; disable Submit & Update
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGrid();
            btnSubmit.IsEnabled = false;
            btnUpdate.IsEnabled = false;
        }

        // Reload grid rows
        private void RefreshGrid()
        {
            CarGrid.ItemsSource = null;
            CarGrid.ItemsSource = crud.GetCars();
        }

        // Clear every text box
        private void ClearInputs()
        {
            txtId.Clear();
            txtVin.Clear();
            txtMake.Clear();
            txtModel.Clear();
            txtYear.Clear();
            txtPrice.Clear();
        }

        // Clear form, show next ID, & enable Submit
        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            ClearInputs();
            List<Car> cars = crud.GetCars();

            // Empty table: first ID equals 1
            if (cars.Count == 0)
            {
                txtId.Text = "1";
            }
            else
            {
                txtId.Text = (cars.Max(x => x.CarId) + 1).ToString();
            }

            CarGrid.SelectedIndex = -1;
            btnSubmit.IsEnabled = true;
            btnUpdate.IsEnabled = false;
        }

        // Validate input & add new car
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Car? car = ReadCar();
            if (car != null)
            {
                crud.AddCar(car);
                RefreshGrid();
                ClearInputs();
                btnSubmit.IsEnabled = false;
            }
        }

        // Fill form using selected row & enable Update
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            Car? car = CarGrid.SelectedItem as Car;
            if (car != null)
            {
                txtId.Text = car.CarId.ToString();
                txtVin.Text = car.Vin;
                txtMake.Text = car.Make;
                txtModel.Text = car.Model;
                txtYear.Text = car.Year.ToString();
                txtPrice.Text = car.Price.ToString();
                btnUpdate.IsEnabled = true;
                btnSubmit.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Select a car in the grid first.");
            }
        }

        // Confirm & delete selected car
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Car? car = CarGrid.SelectedItem as Car;
            if (car == null)
            {
                MessageBox.Show("Select a car in the grid first.");
                return;
            }

            MessageBoxResult answer = MessageBox.Show("Delete " + car.Year + " " + car.Make + " " + car.Model + "?", "Confirm Delete", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.Yes)
            {
                crud.DeleteCar(car.CarId);
                RefreshGrid();
                ClearInputs();
                btnUpdate.IsEnabled = false;
                btnSubmit.IsEnabled = false;
            }
        }

        // Validate & save selected car changes
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Car? car = ReadCar();
            if (car != null)
            {
                crud.UpdateCar(car.CarId, car);
                RefreshGrid();
                ClearInputs();
                btnUpdate.IsEnabled = false;
            }
        }

        // Build car using form values; invalid input shows message & returns null
        private Car? ReadCar()
        {
            int year;
            double price;
            int maxYear = DateTime.Now.Year + 1;

            // Require ID, VIN, make, & model
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtVin.Text) ||
                string.IsNullOrWhiteSpace(txtMake.Text) || string.IsNullOrWhiteSpace(txtModel.Text))
            {
                MessageBox.Show("Fill in VIN, make, and model.");
                return null;
            }

            // Check year & price ranges
            if (!Int32.TryParse(txtYear.Text, out year) || year < 1900 || year > maxYear)
            {
                MessageBox.Show("Enter a year from 1900 to " + maxYear + ".");
                return null;
            }
            if (!Double.TryParse(txtPrice.Text, out price) || price < 0 || Double.IsNaN(price) || Double.IsInfinity(price))
            {
                MessageBox.Show("Enter a price of 0 or more.");
                return null;
            }

            Car car = new Car();
            car.CarId = Int32.Parse(txtId.Text);
            car.Vin = txtVin.Text.Trim();
            car.Make = txtMake.Text.Trim();
            car.Model = txtModel.Text.Trim();
            car.Year = year;
            car.Price = price;

            return car;
        }
    }
}
