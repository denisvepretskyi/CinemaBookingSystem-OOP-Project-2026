using CinemaBookingSystem.Core.Data;
using CinemaBookingSystem.Core.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Services
{
    public static class AppServices
    {
        public static CinemaCatalogManager CinemaCatalogService { get; }
        public static AuthorizationManager AuthorizationService { get; }
        public static BookingManager BookingService { get; }
        public static MovieManager MovieService { get; }
        public static SessionManager SessionService { get; }
        public static StatisticManager StatisticService { get; }
        public static HallManager HallService { get; }
        public static SessionValidator SessionValidator { get; }




        static AppServices()
        {
            SessionValidator = new SessionValidator(AppData.Movies, AppData.Sessions, AppData.Orders);

            CinemaCatalogService = new CinemaCatalogManager(AppData.Cinemas, AppData.Halls);
            AuthorizationService = new AuthorizationManager(AppData.Users, AppData.Orders);
            BookingService = new BookingManager(AppData.Orders, AppData.Sessions, AppData.Halls, AppData.Users);
            MovieService = new MovieManager(AppData.Movies, AppData.Sessions);
            SessionService = new SessionManager(AppData.Sessions, SessionValidator, AppData.Movies, AppData.Cinemas, AppData.Halls);
            StatisticService = new StatisticManager(AppData.Orders, AppData.Sessions, AppData.Movies, AppData.Cinemas);
            HallService = new HallManager();
        }
    }
}
