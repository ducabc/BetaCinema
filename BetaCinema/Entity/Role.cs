﻿namespace BetaCinema.Entity
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
