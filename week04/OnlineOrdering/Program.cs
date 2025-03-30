using System;
using System;
using System.Collections.Generic;
public class Product
{
    private string _name;
    private string _productId;
    private decimal _price;
    private int _quantity;

    public Product(string name, string productId, decimal price, int quantity)
    {
        _name = name;
        _productId = productId;
        _price = price;
        _quantity = quantity;
    }

    public string Name { get { return _name; } set { _name = value; } }
    public string ProductId { get { return _productId; } set { _productId = value; } }
    public decimal Price { get { return _price; } set { _price = value; } }
    public int Quantity { get { return _quantity; } set { _quantity = value; } }

    public decimal CalculateTotalCost()
    {
        return _price * _quantity;
    }
}

public class Address
{
    private string _streetAddress;
    private string _city;
    private string _stateProvince;
    private string _country;

    public Address(string streetAddress, string city, string stateProvince, string country)
    {
        _streetAddress = streetAddress;
        _city = city;
        _stateProvince = stateProvince;
        _country = country;
    }

    public string StreetAddress { get { return _streetAddress; } set { _streetAddress = value; } }
    public string City { get { return _city; } set { _city = value; } }
    public string StateProvince { get { return _stateProvince; } set { _stateProvince = value; } }
    public string Country { get { return _country; } set { _country = value; } }

    public bool IsInUSA()
    {
        return _country.ToUpper() == "USA" || _country.ToUpper() == "UNITED STATES" || _country.ToUpper() == "UNITED STATES OF AMERICA";
    }

    public string GetFullAddress()
    {
        return $"{_streetAddress}\n{_city}, {_stateProvince}\n{_country}";
    }
}

public class Customer
{
    private string _name;
    private Address _address;

    public Customer(string name, Address address)
    {
        _name = name;
        _address = address;
    }

    public string Name { get { return _name; } set { _name = value; } }
    public Address Address { get { return _address; } set { _address = value; } }

    public bool IsInUSA()
    {
        return _address.IsInUSA();
    }
}

public class Order
{
    private List<Product> _products;
    private Customer _customer;

    public Order(Customer customer)
    {
        _customer = customer;
        _products = new List<Product>();
    }

    public Customer Customer { get { return _customer; } set { _customer = value; } }
    
    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public decimal CalculateTotalCost()
    {
        decimal productTotal = 0;
        foreach (Product product in _products)
        {
            productTotal += product.CalculateTotalCost();
        }

        decimal shippingCost = _customer.IsInUSA() ? 5 : 35;
        
        return productTotal + shippingCost;
    }

    public string GetPackingLabel()
    {
        string packingLabel = "PACKING LABEL\n";
        foreach (Product product in _products)
        {
            packingLabel += $"{product.Name} (ID: {product.ProductId})\n";
        }
        return packingLabel;
    }

    public string GetShippingLabel()
    {
        string shippingLabel = "SHIPPING LABEL\n";
        shippingLabel += $"{_customer.Name}\n";
        shippingLabel += _customer.Address.GetFullAddress();
        return shippingLabel;
    }

}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Online Ordering System\n");

        Product laptop = new Product("Gaming Laptop", "TECH001", 999.99m, 1);
        Product headphones = new Product("Wireless Headphones", "AUDIO100", 149.99m, 2);
        Product mouse = new Product("Ergonomic Mouse", "TECH056", 45.50m, 1);
        Product keyboard = new Product("Mechanical Keyboard", "TECH073", 89.99m, 1);
        Product monitor = new Product("4K Monitor", "TECH210", 349.99m, 1);

        Address usAddress = new Address("123 Tech Lane", "Silicon Valley", "CA", "USA");
        Address internationalAddress = new Address("42 Innovation Street", "Toronto", "Ontario", "Canada");

        Customer johnPork = new Customer("John Pork", usAddress);
        Customer vanessaDominguez = new Customer("Vanessa Dominguez", internationalAddress);

        Order order1 = new Order(johnPork);
        order1.AddProduct(laptop);
        order1.AddProduct(headphones);
        order1.AddProduct(mouse);

        Order order2 = new Order(vanessaDominguez);
        order2.AddProduct(monitor);
        order2.AddProduct(keyboard);

        Console.WriteLine("\n===== ORDER 1 =====");
        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine(order1.GetShippingLabel());
        Console.WriteLine($"\nTotal Price: ${order1.CalculateTotalCost()}");

        Console.WriteLine("\n===== ORDER 2 =====");
        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine(order2.GetShippingLabel());
        Console.WriteLine($"\nTotal Price: ${order2.CalculateTotalCost()}");
    }
}