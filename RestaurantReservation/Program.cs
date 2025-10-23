using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Exceptions;
using RestaurantReservation.Services;

namespace RestaurantReservation;

class Program
{
    static async Task Main(string[] args)
    {
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, loggerFactory);

        var mapper = config.CreateMapper();
        await using var context = new RestaurantReservationDbContext();
        var customerRepository = new CustomerRepository(context);
        var employeeRepository = new EmployeeRepository(context);
        var menuItemRepository = new MenuItemRepository(context);
        var orderRepository = new OrderRepository(context);
        var orderItemRepository = new OrderItemRepository(context);
        var reservationRepository = new ReservationRepository(context);
        var restaurantRepository = new RestaurantRepository(context);
        var tableRepository = new TableRepository(context);
        var unitOfWork = new UnitOfWork(context);
        
        var customerService = new CustomerService(customerRepository, mapper);
        var restaurantService = new RestaurantService(restaurantRepository, mapper);
        var employeeService = new EmployeeService(employeeRepository, restaurantService, mapper);
        var menuItemService = new MenuItemService(menuItemRepository, restaurantService, mapper);
        var reservationService = new ReservationService(unitOfWork, customerService, restaurantService, mapper);
        var orderService = new OrderService(orderRepository, employeeService, reservationService, mapper);
        var orderItemService = new OrderItemService(orderItemRepository,menuItemService, mapper);
        var tableService = new TableService(tableRepository,restaurantService, mapper);

        try
        {
            #region Customer

            var createCustomerRequest = new CustomerRequest("Karam2", "Hazzaa2", "Karam2@gmail.com", "0593947678");
            var createdCustomer = await customerService.CreateCustomerAsync(createCustomerRequest);
            var customer = await customerRepository.GetByIdAsync(createdCustomer.Id);
            Console.WriteLine($"Customer: {customer?.FirstName} {customer?.LastName} {customer?.Email}");
            var updateCustomerRequest = new CustomerRequest("Karam4", "Rami", null, null);
            await customerService.UpdateCustomerByIdAsync(createdCustomer.Id, updateCustomerRequest);
            Console.WriteLine($"Customer After Update: {customer?.FirstName} {customer?.LastName} {customer?.Email}");
            var deletedCustomerResult = await customerService.DeleteCustomerByIdAsync(createdCustomer.Id);
            if (deletedCustomerResult)
            {
                Console.WriteLine("The customer has been deleted successfully");
            }
            else
            {
                Console.WriteLine("Not found");
            }

            #endregion

            #region Employee

            var createEmployeeRequest = new EmployeeRequest("Karam2", "Hazzaa2", "Manager", 1);
            var createdEmployee = await employeeService.CreateEmployeeAsync(createEmployeeRequest);
            var employee = await employeeRepository.GetByIdAsync(createdEmployee.Id);
            Console.WriteLine($"Employee: {employee?.FirstName} {employee?.LastName} {employee?.Position}");
            var updateEmployeeRequest = new EmployeeRequest("Karam4", "Rami", null, null);
            await employeeService.UpdateEmployeeByIdAsync(createdEmployee.Id, updateEmployeeRequest);
            Console.WriteLine($"Employee After Update: {employee?.FirstName} {employee?.LastName} {employee?.Position}");
            var deletedEmployeeResult = await employeeService.DeleteEmployeeByIdAsync(createdEmployee.Id);
            if (deletedEmployeeResult)
            {
                Console.WriteLine("The employee has been deleted successfully");
            }
            else
            {
                Console.WriteLine("Not found");
            }

            #endregion

            #region MenuItem

            var createMenuItemRequest = new MenuItemRequest("Fototshini", "Has alot of cheese", 30.5m, 1);
            var createdMenuItem = await menuItemService.CreateMenuItemAsync(createMenuItemRequest);
            var menuItem = await menuItemRepository.GetByIdAsync(createdMenuItem.Id);
            Console.WriteLine($"MenuItem: {menuItem?.Name} {menuItem?.Description} {menuItem?.Price}");
            var updateMenuItemRequest = new MenuItemRequest("Fototshini updated", "30 gram cheese", 10.5m, null);
            await menuItemService.UpdateMenuItemByIdAsync(createdMenuItem.Id, updateMenuItemRequest);
            Console.WriteLine($"MenuItem After Update: {menuItem?.Name} {menuItem?.Description} {menuItem?.Price}");
            var deletedMenuItemResult = await menuItemService.DeleteMenuItemByIdAsync(createdMenuItem.Id);
            if (deletedMenuItemResult)
            {
                Console.WriteLine("The menu item has been deleted successfully");
            }
            else
            {
                Console.WriteLine("Not found");
            }

            #endregion

            #region OrderItem

            var createOrderItemRequest = new OrderItemRequest(1, 1, 2, null);
            var createdOrderItem = await orderItemService.CreateOrderItemAsync(createOrderItemRequest);
            var orderItem = await orderItemRepository.GetByIdAsync(createdOrderItem.Id);
            Console.WriteLine($"OrderItem: {orderItem?.Id}, {orderItem?.MenuItem}, {orderItem?.OrderId}, {orderItem?.Quantity}, {orderItem?.Notes}");
            var updateOrderItemRequest = new OrderItemRequest(null, null, 3, null);
            await orderItemService.UpdateOrderItemByIdAsync(createdOrderItem.Id, updateOrderItemRequest);
            Console.WriteLine($"OrderItem After Update: {orderItem?.Id}, {orderItem?.MenuItem}, {orderItem?.OrderId}, {orderItem?.Quantity}, {orderItem?.Notes} ");
            var deletedOrderItemResult = await orderItemService.DeleteOrderItemByIdAsync(createdOrderItem.Id);
            if (deletedOrderItemResult)
            {
                Console.WriteLine("The order item has been deleted successfully");
            }
            else
            {
                Console.WriteLine("Not found");
            }

            #endregion

            #region Order

            var createOrderRequest = new OrderRequest(1, 1, 400m, 0, 0);
            var createdOrder = await orderService.CreateOrderAsync(createOrderRequest);
            var order = await orderRepository.GetByIdAsync(createdOrder.Id);
            Console.WriteLine($"Order: {order?.Id}, {order?.ReservationId}, {order?.EmployeeId}, {order?.Discount}, {order?.Tax}");
            var updateOrderRequest = new OrderRequest(null, 2, 400m, 15, 0);
            await orderService.UpdateOrderByIdAsync(createdOrder.Id, updateOrderRequest);
            Console.WriteLine($"Order After Update: {order?.Id}, {order?.ReservationId}, {order?.EmployeeId}, {order?.Discount}, {order?.Tax}");
            var deletedOrderResult = await orderService.DeleteOrderByIdAsync(createdOrder.Id);
            if (deletedOrderResult)
            {
                Console.WriteLine("The order has been deleted successfully");
            }
            else
            {
                Console.WriteLine("Not found");
            }


            #endregion

            #region Restaurant

            var createRestaurantRequest = new RestaurantRequest("From Console Restaurant", "Console", "0599", "10PM/11AM");
            var createdRestaurant = await restaurantService.CreateRestaurantAsync(createRestaurantRequest);
            var restaurant = await restaurantRepository.GetByIdAsync(createdRestaurant.Id);
            Console.WriteLine($"Restaurant: {restaurant?.Id}, {restaurant?.Name}, {restaurant?.Address}, {restaurant?.PhoneNumber}, {restaurant?.OpeningHours}");
            var updateRestaurantRequest = new RestaurantRequest("From Console Updated Restaurant", "Updated", null,null);
            await restaurantService.UpdateRestaurantByIdAsync(createdRestaurant.Id, updateRestaurantRequest);
            Console.WriteLine($"Restaurant After Update: {restaurant?.Id}, {restaurant?.Name}, {restaurant?.Address}, {restaurant?.PhoneNumber}, {restaurant?.OpeningHours}");
            var deletedRestaurantResult = await restaurantService.DeleteRestaurantByIdAsync(1001);
            if (deletedRestaurantResult)
            {
                Console.WriteLine("The restaurant has been deleted successfully");
            }
            else
            {
                Console.WriteLine("Not found");
            }
            

            #endregion

            #region Table

            var createTableRequest = new TableRequest(6, 1, 5);
            var createdTable = await tableService.CreateTableAsync(createTableRequest);
            var table = await tableRepository.GetByIdAsync(createdTable.Id);
            Console.WriteLine($"Table: {table?.TableNumber}, {table?.RestaurantId}, {table?.Capacity}");
            var updateTableRequest = new TableRequest(null, null, 7);
            await tableService.UpdateTableByIdAsync(createdTable.Id, updateTableRequest);
            Console.WriteLine($"Table After Update: {table?.TableNumber}, {table?.RestaurantId}, {table?.Capacity}");
            var deletedTableResult = await tableService.DeleteTableByIdAsync(createdTable.Id);
            if (deletedTableResult)
            {
                Console.WriteLine("The table has been deleted successfully");
            }
            else
            {
                Console.WriteLine("Not found");
            }

            #endregion

            #region Reservation

            var createReservationRequest = new ReservationRequest(3, 1, 8);
            var createdReservation = await reservationService.CreateReservationAsync(createReservationRequest);
            var reservation = await reservationRepository.GetByIdAsync(createdReservation.Id);
            Console.WriteLine($"Reservation: {reservation?.Id}, {reservation?.CustomerId}, {reservation?.RestaurantId}, {reservation?.PartySize}");
            var updateReservationRequest = new ReservationRequest(2,3, 5);
            await reservationService.UpdateReservationByIdAsync(1004, updateReservationRequest);
            Console.WriteLine($"Reservation After Update: {reservation?.Id}, {reservation?.CustomerId}, {reservation?.RestaurantId}, {reservation?.PartySize}");
            var deletedReservationResult = await reservationService.DeleteReservationByIdAsync(1003);
            if (deletedReservationResult)
            {
                Console.WriteLine("The reservation has been deleted successfully");
            }
            else
            {
                Console.WriteLine("Not found");
            }

            #endregion
        } catch (NotFoundException ex)
        {
            Console.Error.WriteLine("--- OPERATION FAILED ---");
            Console.Error.WriteLine($"Error Type: Not Found");
            Console.Error.WriteLine($"Details: {ex.Message}");
        }
        catch (AlreadyExistsException ex)
        {
            Console.Error.WriteLine("--- OPERATION FAILED ---");
            Console.Error.WriteLine($"Error Type: Already Exists");
            Console.Error.WriteLine($"Details: {ex.Message}");
        }
        catch (NoAvailableTablesException ex)
        {
            Console.Error.WriteLine("--- OPERATION FAILED ---");
            Console.Error.WriteLine($"Error Type: No Tables Available");
            Console.Error.WriteLine($"Details: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("--- FATAL / UNEXPECTED ERROR ---");
            Console.Error.WriteLine($"Error Type: Unhandled Exception");
            Console.Error.WriteLine($"Details: {ex.Message}");
            Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
        }

        #region ListManagers
        var managers = await employeeService.ListAllManagers();
        foreach (var manager in managers)
        {
            Console.WriteLine($"Id: {manager.Id}, FirstName: {manager.FirstName}," +
                              $" LastName: {manager.LastName} Position: {manager.Position}");
        }
        #endregion
        
        #region GetReservationsByCustomerId
        
        var reservations = await reservationService.GetReservationsByCustomerId(1);
        Console.WriteLine($"CustomerId {1} has {reservations.Count} reservations: ");
        foreach (var reservation in reservations)
        {
            Console.WriteLine($"ReservationId: {reservation.Id}, RestaurantId: {reservation.RestaurantId}, PartySize: {reservation.PartySize}");
        }
        #endregion
        
        #region ListOrdersAndMenuItems
        
        var orders = await orderService.ListOrdersAndMenuItemsForReservationIdAsync(1);
        Console.WriteLine($"ReservationId {1} has {orders.Count} orders:");
        foreach (var order in orders)
        {
            Console.WriteLine($"OrderId {order.OrderId} is:");
            foreach (var item in order.MenuItems)
                Console.WriteLine($" - {item.ItemName} x {item.Quantity} ({item.Notes})");
        }
        
        #endregion
        
        #region ListOrdersAndMenuItems
        
        var menuItems = await orderService.ListOrderedMenuItemsForReservationIdAsync(1);
        Console.WriteLine($"ReservationId 1 has: ");
        foreach (var menuItem in menuItems)
        {
            Console.WriteLine($" - {menuItem.ItemName} x {menuItem.Quantity} ({menuItem.Notes})");
        }
        
        #endregion
        
        #region CalculateAverageOrderAmount
        
        Console.WriteLine($"Avg for employeeId 1 = {await orderService.CalculateAverageOrderAmountForEmployeeIdAsync(1)}");
        
        #endregion
        
        #region ListReservationsWithDetails
        
        var reservationsWithDetails = await context.ReservationWithDetails.ToListAsync();
        foreach (var reservation in reservationsWithDetails)
        {
            Console.WriteLine($"ReservationId: {reservation.ReservationId}, CustomerId: {reservation.CustomerId}," +
                              $" CustomerName: {reservation.CustomerName}, RestaurantId: {reservation.ReservationId}" +
                              $", RestaurantName: {reservation.RestaurantName}, RestaurantAddress: {reservation.RestaurantAddress}");
        }
        #endregion
        
        #region ListEmployeeWithDetails
        
        var employeesWithDetails = await context.EmployeeWithDetails.ToListAsync();
        foreach (var employee in employeesWithDetails)
        {
            Console.WriteLine($"EmployeeId: {employee.EmployeeId}, EmployeeName: {employee.EmployeeName}," +
                              $" Position: {employee.Position}, RestaurantName: {employee.RestaurantId}" +
                              $", RestaurantName: {employee.RestaurantName}, RestaurantAddress: {employee.RestaurantAddress}");
        }
        
        #endregion
        
        #region GetRestaurantRevenue
        
        Console.WriteLine($"Revenue for restaurantId 1 = {await restaurantService.CalculateRestaurantRevenueAsync(1)}");
        
        #endregion
        
        #region ListCustomersReservationExceedsPartySize
        
        var customers = await reservationService.ListCustomersReservationExceedsPartySize(3);
        
        foreach (var customer in customers)
        {
            Console.WriteLine($"Id: {customer.Id}, Name: {customer.Name}, Email: {customer.Email}, PartySize: {customer.PartySize}");
        }
        
        #endregion
    }
}

