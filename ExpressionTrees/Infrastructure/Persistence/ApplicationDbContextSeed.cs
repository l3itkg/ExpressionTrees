using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static void Seed(DbContext context)
        {
            if (context is IApplicationDbContext applicationContext && !applicationContext.StaffCategory.Any())
            {
                applicationContext.StaffCategory.Add(new StaffCategory
                {
                    IsHighest = true,
                    Name = "Программист Senior",
                    Staff = new List<Staff>
                    {
                        new Staff
                        {
                            FirstName = "Олег",
                            IsActive = true,
                            IsOfficial = true,
                            LastName = "Сорокин"
                        },
                        new Staff
                        {
                            FirstName = "Ольга",
                            LastName = "Савина",
                            IsOfficial = true,
                            IsActive = false
                        },
                        new Staff
                        {
                            IsActive = true,
                            IsOfficial = false,
                            FirstName = "Василий",
                            LastName = "Мамонтов"
                        }
                    }
                });

                applicationContext.StaffCategory.Add(new StaffCategory
                {
                    IsHighest = false,
                    Name = "Программист Middle",
                    Staff = new List<Staff>
                    {
                        new Staff
                        {
                            FirstName = "Богдан",
                            IsActive = true,
                            IsOfficial = true,
                            LastName = "Гусев"
                        },
                        new Staff
                        {
                            FirstName = "Тимофей",
                            LastName = "Носов",
                            IsOfficial = true,
                            IsActive = false
                        },
                        new Staff
                        {
                            IsActive = false,
                            IsOfficial = false,
                            FirstName = "Андрей",
                            LastName = "Зимин"
                        }
                    }
                });

                applicationContext.StaffCategory.Add(new StaffCategory
                {
                    IsHighest = false,
                    Name = "Программист Junior",
                    Staff = new List<Staff>
                    {
                        new Staff
                        {
                            FirstName = "Яков",
                            IsActive = true,
                            IsOfficial = true,
                            LastName = "Федотов"
                        },
                        new Staff
                        {
                            FirstName = "Степан",
                            LastName = "Королёв",
                            IsOfficial = true,
                            IsActive = true
                        },
                        new Staff
                        {
                            IsActive = true,
                            IsOfficial = true,
                            FirstName = "Виталий",
                            LastName = "Ковалёв"
                        },
                        new Staff
                        {
                            IsActive = true,
                            IsOfficial = false,
                            FirstName = "Виктория",
                            LastName = "Авдеева"
                        },
                        new Staff
                        {
                            IsActive = true,
                            IsOfficial = true,
                            FirstName = "Светлана",
                            LastName = "Куликова"
                        }
                    }
                });
            }


            context.SaveChanges();
        }
    }
}