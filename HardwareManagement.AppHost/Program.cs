var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.HardwareManagement_Server>("hardwaremanagement.server");

//builder.AddProject<Projects.HardwareManagement_Client>("hardwaremanagement.client");

builder.Build().Run();
