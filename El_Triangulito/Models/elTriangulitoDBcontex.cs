﻿using Microsoft.EntityFrameworkCore;


namespace El_Triangulito.Models
{
    public class elTriangulitoDBcontex : DbContext
    {

        public elTriangulitoDBcontex(DbContextOptions options) : base(options)
        {

        }

        public DbSet<comentario_pedidos> comentario_pedidos { get; set; }
        public DbSet<estados> estados { get; set; }
        public DbSet<mesas> mesas { get; set; }
        public DbSet<Detalle_Pedido> Detalle_Pedido { get; set; }
        public DbSet<Promociones> Promociones { get; set; }
        public DbSet<items_promo> items_promo { get; set; }
        public DbSet<combos> combos { get; set; }
        public DbSet<categorias> categorias { get; set; }
        public DbSet<items_combo> items_combo { get; set; }
        public DbSet<items_menu> items_menu { get; set; }
        public DbSet<detalle_fac> detalle_fac { get; set; }
        public DbSet<Cuenta> Cuenta { get; set; }
        public DbSet<encabezado_fac> encabezado_fac { get; set; }


    }
}
