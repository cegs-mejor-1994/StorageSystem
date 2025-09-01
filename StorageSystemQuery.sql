USE [StorageSystem_DB]
GO

IF NOT EXISTS (SELECT 1 FROM Clients WHERE Nit = '66574890')
BEGIN
    INSERT INTO Clients (Code, Nit, Name, Address, Phone, City, State)
    VALUES ('1', '66574890', 'Salsamentaria Valeria', 'Carrera 12 46 14', '3214567845', 'Cali', 'Disponible');
END;