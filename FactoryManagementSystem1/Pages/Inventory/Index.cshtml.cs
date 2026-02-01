@page
@model IndexModel
@{
    ViewData["Title"] = "Dashboard";
}

<div class="container-fluid">
    <h1 class="mb-4">Dashboard</h1>

    <!-- Statistics Cards -->
    <div class="row mb-4">
        <!-- Inventory Card -->
        <div class="col-md-3 mb-3">
            <div class="card text-white bg-primary">
                <div class="card-body">
                    <h5 class="card-title">Total Inventory Items</h5>
                    <h2 class="mb-0">@Model.TotalInventoryItems</h2>
                </div>
            </div>
        </div>

        <!-- Low Stock Card -->
        <div class="col-md-3 mb-3">
            <div class="card text-white @(Model.LowStockCount > 0 ? "bg-danger" : "bg-success")">
                <div class="card-body">
                    <h5 class="card-title">Low Stock Items</h5>
                    <h2 class="mb-0">@Model.LowStockCount</h2>
                </div>
            </div>
        </div>

        <!-- Total Work Orders Card -->
        <div class="col-md-3 mb-3">
            <div class="card text-white bg-info">
                <div class="card-body">
                    <h5 class="card-title">Total Work Orders</h5>
                    <h2 class="mb-0">@Model.TotalWorkOrders</h2>
                </div>
            </div>
        </div>

        <!-- Completed Work Orders Card -->
        <div class="col-md-3 mb-3">
            <div class="card text-white bg-success">
                <div class="card-body">
                    <h5 class="card-title">Completed Work Orders</h5>
                    <h2 class="mb-0">@Model.CompletedWorkOrders</h2>
                </div>
            </div>
        </div>
    </div>

    <!-- Work Order Status Breakdown -->
    <div class="row mb-4">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header">
                    <h5 class="mb-0">Work Order Status</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-2 text-center">
                            <div class="p-3 border rounded">
                                <h6 class="text-muted">Draft</h6>
                                <h4>@Model.DraftWorkOrders</h4>
                            </div>
                        </div>
                        <div class="col-md-2 text-center">
                            <div class="p-3 border rounded">
                                <h6 class="text-muted">Submitted</h6>
                                <h4>@Model.SubmittedWorkOrders</h4>
                            </div>
                        </div>
                        <div class="col-md-2 text-center">
                            <div class="p-3 border rounded">
                                <h6 class="text-muted">Approved</h6>
                                <h4>@Model.ApprovedWorkOrders</h4>
                            </div>
                        </div>
                        <div class="col-md-2 text-center">
                            <div class="p-3 border rounded">
                                <h6 class="text-muted">In Progress</h6>
                                <h4>@Model.InProgressWorkOrders</h4>
                            </div>
                        </div>
                        <div class="col-md-2 text-center">
                            <div class="p-3 border rounded bg-success text-white">
                                <h6>Completed</h6>
                                <h4>@Model.CompletedWorkOrders</h4>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Recent Activity -->
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header">
                    <h5 class="mb-0">Recent Activity</h5>
                </div>
                <div class="card-body">
                    @if (Model.RecentAuditLogs.Any())
                    {
                        <div class="table-responsive">
                            <table class="table table-sm">
                                <thead>
                                    <tr>
                                        <th>Time</th>
                                        <th>Action</th>
                                        <th>Entity</th>
                                        <th>Details</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    @foreach (var log in Model.RecentAuditLogs)
                                    {
                                        <tr>
                                            <td>@log.TimestampUtc.ToString("yyyy-MM-dd HH:mm:ss")</td>
                                            <td><span class="badge bg-secondary">@log.Action</span></td>
                                            <td>@log.EntityName</td>
                                            <td>@log.Details</td>
                                        </tr>
                                    }
                                </tbody>
                            </table>
                        </div>
                    }
                    else
                    {
                        <p class="text-muted">No recent activity.</p>
                    }
                </div>
            </div>
        </div>
    </div>
</div>@page
@model IndexModel
@{
    ViewData["Title"] = "Dashboard";
}

<div class="container-fluid">
    <h1 class="mb-4">Dashboard</h1>

    <!-- Statistics Cards -->
    <div class="row mb-4">
        <!-- Inventory Card -->
        <div class="col-md-3 mb-3">
            <div class="card text-white bg-primary">
                <div class="card-body">
                    <h5 class="card-title">Total Inventory Items</h5>
                    <h2 class="mb-0">@Model.TotalInventoryItems</h2>
                </div>
            </div>
        </div>

        <!-- Low Stock Card -->
        <div class="col-md-3 mb-3">
            <div class="card text-white @(Model.LowStockCount > 0 ? "bg-danger" : "bg-success")">
                <div class="card-body">
                    <h5 class="card-title">Low Stock Items</h5>
                    <h2 class="mb-0">@Model.LowStockCount</h2>
                </div>
            </div>
        </div>

        <!-- Total Work Orders Card -->
        <div class="col-md-3 mb-3">
            <div class="card text-white bg-info">
                <div class="card-body">
                    <h5 class="card-title">Total Work Orders</h5>
                    <h2 class="mb-0">@Model.TotalWorkOrders</h2>
                </div>
            </div>
        </div>

        <!-- Completed Work Orders Card -->
        <div class="col-md-3 mb-3">
            <div class="card text-white bg-success">
                <div class="card-body">
                    <h5 class="card-title">Completed Work Orders</h5>
                    <h2 class="mb-0">@Model.CompletedWorkOrders</h2>
                </div>
            </div>
        </div>
    </div>

    <!-- Work Order Status Breakdown -->
    <div class="row mb-4">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header">
                    <h5 class="mb-0">Work Order Status</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-2 text-center">
                            <div class="p-3 border rounded">
                                <h6 class="text-muted">Draft</h6>
                                <h4>@Model.DraftWorkOrders</h4>
                            </div>
                        </div>
                        <div class="col-md-2 text-center">
                            <div class="p-3 border rounded">
                                <h6 class="text-muted">Submitted</h6>
                                <h4>@Model.SubmittedWorkOrders</h4>
                            </div>
                        </div>
                        <div class="col-md-2 text-center">
                            <div class="p-3 border rounded">
                                <h6 class="text-muted">Approved</h6>
                                <h4>@Model.ApprovedWorkOrders</h4>
                            </div>
                        </div>
                        <div class="col-md-2 text-center">
                            <div class="p-3 border rounded">
                                <h6 class="text-muted">In Progress</h6>
                                <h4>@Model.InProgressWorkOrders</h4>
                            </div>
                        </div>
                        <div class="col-md-2 text-center">
                            <div class="p-3 border rounded bg-success text-white">
                                <h6>Completed</h6>
                                <h4>@Model.CompletedWorkOrders</h4>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Recent Activity -->
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header">
                    <h5 class="mb-0">Recent Activity</h5>
                </div>
                <div class="card-body">
                    @if (Model.RecentAuditLogs.Any())
                    {
                        <div class="table-responsive">
                            <table class="table table-sm">
                                <thead>
                                    <tr>
                                        <th>Time</th>
                                        <th>Action</th>
                                        <th>Entity</th>
                                        <th>Details</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    @foreach (var log in Model.RecentAuditLogs)
                                    {
                                        <tr>
                                            <td>@log.TimestampUtc.ToString("yyyy-MM-dd HH:mm:ss")</td>
                                            <td><span class="badge bg-secondary">@log.Action</span></td>
                                            <td>@log.EntityName</td>
                                            <td>@log.Details</td>
                                        </tr>
                                    }
                                </tbody>
                            </table>
                        </div>
                    }
                    else
                    {
                        <p class="text-muted">No recent activity.</p>
                    }
                </div>
            </div>
        </div>
    </div>
</div>