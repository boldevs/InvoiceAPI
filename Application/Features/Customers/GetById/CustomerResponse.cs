﻿namespace Application.Features.Customers.GetById
{
    public sealed class CustomerResponse
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
