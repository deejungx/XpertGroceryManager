using System.Linq;
using XpertGroceryManager.Models;

namespace XpertGroceryManager.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Categories.Any())
            {
                var categories = new Category[]
                {
                    new Category{Name="Beverages", Description="coffee/tea, juice, soda"},
                    new Category{Name="Bread/Bakery", Description="sandwich loaves, dinner rolls, tortillas, bagels"},
                    new Category{Name="Dairy", Description="cheeses, eggs, milk, yogurt, butter"},
                    new Category{Name="Dry/Baking Goods", Description="cereals, flour, sugar, pasta, mixes"},
                    new Category{Name="Frozen Foods", Description="waffles, vegetables, individual meals, ice cream"},
                    new Category{Name="Meat", Description="lunch meat, poultry, beef, pork"},
                    new Category{Name="Produce", Description="fruits, vegetables"},
                    new Category{Name="Cleaners", Description="laundry detergent, dishwashing liquid/detergent"},
                    new Category{Name="Paper Goods", Description="paper towels, toilet paper, aluminum foil, sandwich bags"},
                    new Category{Name="Personal Care", Description="shampoo, soap, hand soap, shaving cream"},
                    new Category{Name="Other", Description="baby items, pet items, batteries, greeting cards"},
                };

                foreach (Category c in categories)
                {
                    context.Categories.Add(c);
                }
                context.SaveChanges();
            }

            

             if (!context.Products.Any())
            {
                var products = new Product[]
                {
                    new Product{Name="Nescafe Gold Coffee-50gm", Description="Nescafe Gold is a blend of mountain grown Arabica and Robusta beans.", Price=450.00m, CategoryId=1},
                    new Product{Name="Himalayan Java Coffee - Thamel Roast, 250 gms", Description="Himalayan Java Coffee", Price=825.00m, CategoryId=1},
                    new Product{Name="Himalayan Java Coffee - Medium Roast, 450 gms", Description="Himalayan Java Coffee", Price=1400.00m, CategoryId=1},
                    new Product{Name="Nescafe Gold Coffee - 100gm", Description="Premium, imported, freeze dried soluble coffee powder.", Price=780.00m, CategoryId=1},
                    new Product{Name="Somersby Apple Cider Bottle 250ML", Description="Somersby is Carlsberg Group's biggest selling cider", Price=165.00m, CategoryId=1},
                    new Product{Name="Rakura Supreme CTC Tea, 500gm", Description="Rakura Tea", Price=295.00m, CategoryId=1},
                    new Product{Name="Tokla Tea Bags - 100 Packets", Description="Tokla Tea", Price=165.00m, CategoryId=1},
                
                    new Product{Name="Trisara Bakery Soup Stick Bread, 15 piece", Description="Bread sticks are commonly served with soups", Price=75.00m, CategoryId=2},
                    new Product{Name="European Bakery Brown Bread", Description="", Price=140.00m, CategoryId=2},
                    new Product{Name="Nanglo Brown Bread - 500 gm", Description="Brown Bread", Price=88.00m, CategoryId=2},
                    new Product{Name="Nanglo Croissant - 300gm (5Pcs)", Description="", Price=90.00m, CategoryId=2},
                    new Product{Name="Trisara Bakery Mini Chocolate Doughnut, 4piece", Description="Small portions of chocolate or cocoa is beneficial for heart health.", Price=130.00m, CategoryId=2},
                
                    new Product{Name="Amul Butter-200gm", Description="Spread on Bread, Parantha, Roti, Nans, Sandwiches", Price=255.00m, CategoryId=3},
                    new Product{Name="Amul Butter-500gm", Description="Spread on Bread, Parantha, Roti, Nans, Sandwiches", Price=620.00m, CategoryId=3},
                    new Product{Name="Vegan Dairy Pumpkin Seed Butter, 200ml", Description="Pumpkin seed butter is a great source of zinc and magnesium", Price=600.00m, CategoryId=3},
                    new Product{Name="Ferments Almond Butter, 190gm", Description="Almond butter is high in monounsaturated fats.", Price=775.00m, CategoryId=3},
                    new Product{Name="DDC Yak Cheese - 500 gm", Description="Local DDC Cheese", Price=845.00m, CategoryId=3},
                    new Product{Name="Amul Yummy Plain Cheese Spreed, 200gm", Description="Amul Cheese Spread - Yummy Plain is a good supply of calcium and milk proteins.", Price=175.00m, CategoryId=3},

                    new Product{Name="Weikfield Baking Soda, 100gm", Description="Baking soda is a magical ingredient which works wonderfully with a batter of khaman, dhokla or idli.", Price=52.00m, CategoryId=4},
                    new Product{Name="Blue Cow Condensed Milk - 390 gms", Description="Bluecow Condened Milk is an establish brand in Nepal used by the top bakeries and hotels", Price=190.00m, CategoryId=4},
                    new Product{Name="Kent Syrup Caramel, 624 gm", Description="", Price=390.00m, CategoryId=4},
                    new Product{Name="Weikfield Cocoa Powder, 50gm", Description="Makes delicious and flavourful chocolate sauces, cakes and other chocolate desserts", Price=109.00m, CategoryId=4},
                    new Product{Name="American Garden Strawberry Syrup-680gm", Description="Our strawberry syrup adds a hint of sweetness and freshness when drizzled over any dessert!", Price=575.00m, CategoryId=4},
                    new Product{Name="Morde Compound White - 500 gms", Description="Morde White Compound is a vegetable fat based chocolate", Price=398.00m, CategoryId=4},

                    new Product{Name="McCain French Fries, Favorita 9mm-2.5kg", Description="McCain offers this pack of frozen French fries", Price=935.00m, CategoryId=5},
                    new Product{Name="McCain Smiles-415gm", Description="McCain Smiles are appetizing delicacies that are made up of mashed potatoes", Price=297.00m, CategoryId=5},
                    new Product{Name="Golden Fresh Ready To Cook Veg Momo-40Pc", Description="Frozen Momo", Price=400.00m, CategoryId=5},
                    new Product{Name="European Bakery Veg Momo - 10 Pcs", Description="Frozen Momo", Price=180.00m, CategoryId=5},
                    new Product{Name="McCain Super Wedges-400gm", Description="McCain Super Wedges are crispy coated potatoes", Price=297.00m, CategoryId=5},
                
                    new Product{Name="Norsk Sjomat Norwegian Smoked Salmon, 200 g", Description="", Price=1550.00m, CategoryId=6},
                    new Product{Name="Urban Food Medium Prawns - 500 gms", Description="", Price=1000.00m, CategoryId=6},
                    new Product{Name="Nina & Hager Frozen Trout - Per KG", Description="", Price=1400.00m, CategoryId=6},
                    new Product{Name="Golden Fresh Prawn 16/20 ,1kg", Description="", Price=2100.00m, CategoryId=6},
                    new Product{Name="Gourmet Vienna Pork Ribs- Per Kg", Description="", Price=1080.00m, CategoryId=6},
                    new Product{Name="Gourmet Vienna Pork Slice-200gm", Description="", Price=310.00m, CategoryId=6},

                    new Product{Name="Kiwi, 1kg", Description="", Price=680.00m, CategoryId=7},
                    new Product{Name="Apple ( Fuzi ), 1kg", Description="", Price=280.00m, CategoryId=7},
                    new Product{Name="Avocado, 1kg", Description="Avocados are oval shaped fruits with a thick green and a bumpy, leathery outer skin.", Price=520.00m, CategoryId=7},
                    new Product{Name="Pomegranate, 1kg", Description="", Price=360.00m, CategoryId=7},
                    new Product{Name="Green Apple, 1kg", Description="", Price=520.00m, CategoryId=7},
                    new Product{Name="Mousam, 1kg", Description="", Price=220.00m, CategoryId=7},
                    new Product{Name="Guava, 1kg", Description="", Price=130.00m, CategoryId=7},
                    new Product{Name="American Strawberry, 500gm", Description="", Price=600.00m, CategoryId=7},

                    new Product{Name="Comfort Fabric Conditioner (Sense of Pleasure)-2ltr", Description="", Price=890.00m, CategoryId=8},
                    new Product{Name="Comfort Fabric Conditioner(Touch of Love)-2ltr", Description="Comfort Fabric Conditioner keep your clothes from wearing out too soon.", Price=850.00m, CategoryId=8},
                    new Product{Name="Tide Bar, 250gm (Pack of 3)", Description="New Tide + with Extra Power detergent, now with the added Power of Bar", Price=105.00m, CategoryId=8},
                    new Product{Name="Surf Excel Stain Eraser Bar - 250 g", Description="Removes tough stains such as tea, coffee, turmeric, curry, ketchup", Price=47.00m, CategoryId=8},
                    new Product{Name="Surf Excel Matic Top Load Detergent Powder, 1 kg", Description="Tough stain removal in machine with Surf excel matic", Price=360.00m, CategoryId=8},
                    new Product{Name="Ariel Matic Front Load, 1kg", Description="Best used for front load fully automatic washing machines", Price=400.00m, CategoryId=8},

                    new Product{Name="Paseo Toilet Roll 200's 4Ply Dolphin- 10rolls", Description="", Price=870.00m, CategoryId=9},
                    new Product{Name="Paseo Elegant Toilet Roll 280's, 3Ply-4roll", Description="Toilet Rolls: Paseo Elegant 3 Ply Bathroom Tissue 4 Roll 280 Sheet", Price=380.00m, CategoryId=9},
                    new Product{Name="Paseo Toilet Roll 300's 3Ply-8roll", Description="Hygenic, soft and natural", Price=830.00m, CategoryId=9},

                    new Product{Name="Newlook Suncontrol spf 50 for Normal & Dry skin-100ml", Description="NEWLOOK’s Daily Sun Control PA+++ SPF-50", Price=460.00m, CategoryId=10},
                    new Product{Name="Lifebuoy Total - 95gm", Description="", Price=35.00m, CategoryId=10},
                    new Product{Name="Dettol Soap Regular 125gm (Buy 4 and Get 1 Free)", Description="", Price=320.00m, CategoryId=10},
                    new Product{Name="Naturo Earth Pink Salt Soap For Face And Body, 100gm", Description="Naturo Earth’s Pink Salt Soap is infused with Himalayan Pink Salt", Price=300.00m, CategoryId=10},
                    new Product{Name="Head & Shoulder Smooth And Silky Shampoo 720ml", Description="", Price=690.00m, CategoryId=10},

                    new Product{Name="Black & Green Extra Virgin Avocado Oil, 220ml", Description="", Price=2500.00m, CategoryId=11},
                    new Product{Name="Juas Grass Fed Ghee-1000ml", Description="Raw free range organic pastured ghee. No salt, sugar or preservatives added.", Price=1750.00m, CategoryId=11},
                    new Product{Name="Baby Dove Rich Moisture Head to Toe Wash, 400 ml", Description="A tear-free and hypoallergenic baby wash and shampoo that helps moisturise your baby’s delicate skin to keep it soft.", Price=425.00m, CategoryId=11},
                    new Product{Name="Johnson & Johnson Baby Shampoo No More Tears, 100ml", Description="Johnson's Baby shampoo is as gentle to the eyes as pure water", Price=144.00m, CategoryId=11},
                    new Product{Name="Kennel Adult Dog Food Pouch 1.5Kg", Description="Shinier coats, Healthier skin...", Price=475.00m, CategoryId=11},
                    new Product{Name="BonaCibo Adult Lamb & Rice, 15kg", Description="", Price=5500.00m, CategoryId=11},
                    new Product{Name="Alpha Electric Kettle Stainless Steel Cordless Jug-2Ltr", Description="", Price=730.00m, CategoryId=11},
                    new Product{Name="Baltra Prima Induction Cooker", Description="2000W Energy Saving", Price=4400.00m, CategoryId=11},
                };

                foreach (Product p in products)
                {
                    context.Products.Add(p);
                }
                context.SaveChanges();
            }

            

            if (!context.Customers.Any())
            {
                var customers = new Customer[]
                {
                    new Customer{Name="Reiko Bridewell", MemberNumber="12160609"},
                    new Customer{Name="Ruben Ketchaside", MemberNumber="23665561", Phone="6039991683", Email="rketchaside1@eepurl.com"},
                    new Customer{Name="Jilly Bendall", MemberNumber="60673715", MemberType="Bronze", Phone="1825693497", Address="134 Blackbird Hill", Email="jbendall2@blog.com"},
                    new Customer{Name="Maritsa Wildber	", MemberNumber="20697860", MemberType="Bronze", Phone="3661740034", Address="4780 2nd Circle", Email="mwildber3@imgur.com"},
                    new Customer{Name="Winni Di Iorio", MemberNumber="55966772", MemberType="Gold", Phone="6499054605", Address="902 Pawling Street", Email="wdi4@artisteer.com"},
                    new Customer{Name="Kissiah Gaitone", MemberNumber="56339888"},
                    new Customer{Name="Gabriell Bartolozzi", MemberNumber="96900296", Phone="7412745335", Email="gbartolozzi6@facebook.com"},
                    new Customer{Name="Feodor Hansod", MemberNumber="40625989", Phone="4558027879", Email="fhansod7@harvard.edu"},
                };

                foreach (Customer c in customers)
                {
                    context.Customers.Add(c);
                }
                context.SaveChanges();
            }

            

            if (!context.Stocks.Any())
            {
                var stocks = new Stock[]
                {
                    new Stock{Id=1, Quantity=12},
                    new Stock{Id=2, Quantity=10},
                    new Stock{Id=3, Quantity=10},
                    new Stock{Id=4, Quantity=5},
                    new Stock{Id=7, Quantity=4},
                    new Stock{Id=8, Quantity=15},
                    new Stock{Id=9, Quantity=20},
                    new Stock{Id=10, Quantity=8},
                    new Stock{Id=11, Quantity=22},
                    new Stock{Id=12, Quantity=25},
                    new Stock{Id=13, Quantity=50},
                    new Stock{Id=14, Quantity=46},
                    new Stock{Id=15, Quantity=35},
                    new Stock{Id=16, Quantity=23},
                    new Stock{Id=17, Quantity=22},
                    new Stock{Id=18, Quantity=16},
                    new Stock{Id=19, Quantity=19},
                    new Stock{Id=21, Quantity=12},
                    new Stock{Id=22, Quantity=22},
                    new Stock{Id=25, Quantity=30},
                    new Stock{Id=26, Quantity=21},
                    new Stock{Id=27, Quantity=26},
                    new Stock{Id=28, Quantity=16},
                    new Stock{Id=29, Quantity=6},
                    new Stock{Id=30, Quantity=36},
                    new Stock{Id=32, Quantity=33},
                    new Stock{Id=33, Quantity=22},
                    new Stock{Id=35, Quantity=11},
                    new Stock{Id=37, Quantity=2},
                    new Stock{Id=39, Quantity=26},
                    new Stock{Id=42, Quantity=37},
                    new Stock{Id=45, Quantity=24},
                    new Stock{Id=46, Quantity=15},
                    new Stock{Id=47, Quantity=16},
                    new Stock{Id=48, Quantity=15},
                    new Stock{Id=49, Quantity=11},
                    new Stock{Id=50, Quantity=18},
                    new Stock{Id=52, Quantity=32},
                    new Stock{Id=53, Quantity=19},
                    new Stock{Id=55, Quantity=22},
                    new Stock{Id=57, Quantity=27},
                    new Stock{Id=59, Quantity=26},
                    new Stock{Id=60, Quantity=32},
                    new Stock{Id=61, Quantity=12},
                    new Stock{Id=62, Quantity=16},
                    new Stock{Id=63, Quantity=19},
                    new Stock{Id=65, Quantity=14},
                };

                foreach (Stock s in stocks)
                {
                    context.Stocks.Add(s);
                }
                context.SaveChanges();
            }

            
        }
    }
}