CREATE TABLE Customers (
    CustomerId INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Email TEXT NOT NULL,
    Phone TEXT
);

CREATE TABLE Orders (
    OrderId INTEGER PRIMARY KEY AUTOINCREMENT,
    CustomerId INTEGER NOT NULL,
    OrderDate TEXT NOT NULL,
    Status TEXT NOT NULL,
    TotalAmount REAL NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);

CREATE TABLE OrderItems (
    OrderItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    ProductName TEXT NOT NULL,
    Quantity INTEGER NOT NULL,
    UnitPrice REAL NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);

CREATE TABLE Returns (
    ReturnId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL UNIQUE,
    ReturnDate TEXT NOT NULL,
    Reason TEXT,
    RefundedAmount REAL NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);
