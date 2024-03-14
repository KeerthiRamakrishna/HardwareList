var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.HardwareManagement_Server>("hardwaremanagement.server");

builder.Build().Run();
