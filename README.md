# ComputerStore
Internet Services Project

## Installation

### 1. Clone the Repository
```bash
git clone https://github.com/miloshevska-m/ComputerStore.git
cd ComputerStore
```
### 2. Open the Project
- Open the solution `(.sln)` file in Visual Studio 2022.

### 3. Configure the Database
- Update the connection string in `appsettings.json` under the `"ConnectionStrings"` section:
```json
  "ConnectionStrings": {
  "DefaultConnection": "Data Source=YOUR_SQL_SERVER;Initial Catalog=ComputerStoreDB;Integrated Security=True;Trust Server Certificate=True"
}
```
- Run the following command in the Package Manager Console to apply migrations:
```bash
Update-Database
```

### 5. Add data in Tables
- To add sample data to the database, you can execute the following SQL script:
```sql
INSERT INTO [ComputerStoreDb].[dbo].[Categories] ([Name], [Description])
VALUES
('GPU', 'Graphics cards'),
('Keyboard', 'Computer keyboards'),
('Periphery', 'Peripheral devices'),
('Motherboard', 'Mainboards for PCs'),
('Monitors', 'All types of monitors');

INSERT INTO [ComputerStoreDb].[dbo].[Products] ([Name], [Description], [Price], [Stock], [CategoryId])
VALUES
('NVIDIA RTX 3080', 'High-end graphics card for gaming and rendering', 699.00, 10, 2),
('Corsair K70 Keyboard', 'Mechanical gaming keyboard with RGB lighting', 129.00, 20, 3),
('Logitech MX Master 3', 'Wireless mouse with ergonomic design', 99.00, 15, 4),
('ASUS ROG STRIX B550-F', 'Motherboard for AMD Ryzen processors', 179.00, 8, 5),
('Razer BlackWidow Keyboard', NULL, 90.00, 2, 3);
```
