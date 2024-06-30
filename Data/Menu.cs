namespace HotelManagementSystem.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Menu()
        {
            FoodOrders = new HashSet<FoodOrders>();
        }

        public int ID { get; set; }

        public int? RestaurantID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FoodOrders> FoodOrders { get; set; }

        public virtual Restaurants Restaurants { get; set; }
    }
}
