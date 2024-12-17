﻿using PromoCodeFactory.Core.Domain;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PromoCodeFactory.Core.Domain.Administration
{
    public class Role
        : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        //public ICollection<Employee> Employees { get; set; }
    }
}