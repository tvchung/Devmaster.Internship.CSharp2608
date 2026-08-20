using Ex06.ProductManagement.Helpers;
using Ex06.ProductManagement.Models;
using Ex06.ProductManagement.Repositories;
using Ex06.ProductManagement.Services;
using System.Text;

namespace Ex06.ProductManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            var repository = new ProductRepository();
            var service = new ProductService(repository);
            // Nạp dữ liệu mẫu ban đầu
            service.SeedSampleData();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("================================================");
                Console.WriteLine("        HỆ THỐNG QUẢN LÝ SẢN PHẨM               ");
                Console.WriteLine("================================================");
                Console.WriteLine("1.  Thêm sản phẩm");
                Console.WriteLine("2.  Hiển thị tất cảa sản phẩm");
                Console.WriteLine("3.  Kiểm tra mã sản phẩm");
                Console.WriteLine("4.  Cập nhật sản phẩm");
                Console.WriteLine("5.  Xóa sản phẩm");
                Console.WriteLine("6.  Tìm sản phẩm theo mã");
                Console.WriteLine("7.  Tìm gần đúng theo tên");
                Console.WriteLine("8.  Lọc theo danh mục");
                Console.WriteLine("9.  Lọc sản phẩm sắp hết hàng (<= 5)");
                Console.WriteLine("10. Sắp xếp theo giá tăng dần");
                Console.WriteLine("11. Sắp xếp theo giá giảm dần");
                Console.WriteLine("12. Tính tổng giá trị tồn kho");
                Console.WriteLine("13. Thống kê sản phẩm theo danh mục");
                Console.WriteLine("14. Hiển thị sản phẩm theo nhóm danh mục");
                Console.WriteLine("15. Quản lý tag");
                Console.WriteLine("0.  Thoát chương trình");
                Console.WriteLine("================================================");
                Console.Write("Chọn chức năng (0-15): ");

                string? choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        HandleAddProduct(service);
                        break;
                    case "2":
                        PrintProductList(service.GetAllProducts(), "DANH SÁCH TOÀN BỘ SẢN PHẨM");
                        break;
                    case "3":
                        HandleCheckProductExists(service);
                        break;
                    case "4":
                        HandleUpdateProduct(service);
                        break;
                    case "5":
                        HandleDeleteProduct(service);
                        break;
                    case "6":
                        HandleFindById(service);
                        break;
                    case "7":
                        HandleSearchByName(service);
                        break;
                    case "8":
                        HandleFilterByCategory(service);
                        break;
                    case "9":
                        PrintProductList(service.FilterLowStock(), "DANH SÁCH SẢN PHẨM SẮP HẾT HÀNG (SL <= 5)");
                        break;
                    case "10":
                        PrintProductList(service.SortByPrice(ascending: true), "DANH SÁCH SẮP XẾP THEO GIÁ TĂNG DẦN");
                        break;
                    case "11":
                        PrintProductList(service.SortByPrice(ascending: false), "DANH SÁCH SẮP XẾP THEO GIÁ GIẢM DẦN");
                        break;
                    case "12":
                        Console.WriteLine($"-> Tổng giá trị toàn bộ hàng tồn kho: {service.GetTotalInventoryValue():N0} đ");
                        break;
                    case "13":
                        HandleCategoryStatistics(service);
                        break;
                    case "14":
                        HandleDisplayGroupedByCategory(service);
                        break;
                    case "15":
                        HandleTagManagement(service);
                        break;
                    case "0":
                        Console.WriteLine("Cảm ơn bạn đã sử dụng chương trình!");
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        break;
                }

                Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                Console.ReadKey();
            }
        }
        static void PrintHeader()
        {
            Console.WriteLine($"{"Mã SP",-8} | {"Tên sản phẩm",-25} | {"Danh mục",-15} | {"Giá bán",14} | {"SL",5} | {"Nhà cung cấp",-15} | Tags");
            Console.WriteLine(new string('-', 105));
        }

        static void PrintProductList(IEnumerable<Product> products, string title)
        {
            Console.WriteLine($"=== {title} ===");
            var list = products.ToList();
            if (list.Count == 0)
            {
                Console.WriteLine("(Không có dữ liệu sản phẩm nào)");
                return;
            }

            PrintHeader();
            foreach (var p in list)
            {
                Console.WriteLine(p);
            }
            Console.WriteLine(new string('-', 105));
            Console.WriteLine($"Tổng số lượng: {list.Count} sản phẩm.");
        }

        static void HandleAddProduct(ProductService s)
        {
            Console.WriteLine("=== THÊM MỚI SẢN PHẨM ===");
            string id = InputHelper.ReadNonEmptyString("Nhập mã sản phẩm: ");
            if (s.CheckIdExists(id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Lỗi] Mã sản phẩm '{id}' đã tồn tại trong hệ thống!");
                Console.ResetColor();
                return;
            }

            var product = new Product
            {
                ProductId = id,
                ProductName = InputHelper.ReadNonEmptyString("Nhập tên sản phẩm: "),
                Category = InputHelper.ReadNonEmptyString("Nhập danh mục: "),
                Price = InputHelper.ReadDecimal("Nhập đơn giá: ", 0),
                Quantity = InputHelper.ReadInt("Nhập số lượng tồn kho: ", 0),
                Supplier = InputHelper.ReadNonEmptyString("Nhập nhà cung cấp: "),
                Tags = InputHelper.ReadTags("Nhập các tag (ngăn cách bởi dấu phẩy, vd: laptop, gaming): ")
            };

            s.AddProduct(product);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-> Thêm sản phẩm thành công!");
            Console.ResetColor();
        }

        static void HandleCheckProductExists(ProductService s)
        {
            string id = InputHelper.ReadNonEmptyString("Nhập mã sản phẩm cần kiểm tra: ");
            if (s.CheckIdExists(id))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"-> Mã '{id}' ĐÃ TỒN TẠI trong hệ thống.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"-> Mã '{id}' CHƯA TỒN TẠI trong hệ thống.");
                Console.ResetColor();
            }
        }

        static void HandleUpdateProduct(ProductService s)
        {
            Console.WriteLine("=== CẬP NHẬT SẢN PHẨM ===");
            string id = InputHelper.ReadNonEmptyString("Nhập mã sản phẩm cần sửa: ");
            var product = s.GetById(id);
            if (product == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Không tìm thấy sản phẩm với mã: {id}");
                Console.ResetColor();
                return;
            }

            Console.WriteLine($"Đang cập nhật cho: {product.ProductName}");
            product.ProductName = InputHelper.ReadNonEmptyString($"Tên mới (cũ: {product.ProductName}): ");
            product.Category = InputHelper.ReadNonEmptyString($"Danh mục mới (cũ: {product.Category}): ");
            product.Price = InputHelper.ReadDecimal($"Giá mới (cũ: {product.Price:N0}): ");
            product.Quantity = InputHelper.ReadInt($"Số lượng mới (cũ: {product.Quantity}): ");
            product.Supplier = InputHelper.ReadNonEmptyString($"Nhà cung cấp mới (cũ: {product.Supplier}): ");
            product.Tags = InputHelper.ReadTags("Tags mới (ngăn cách bằng dấu phẩy): ");

            s.UpdateProduct(product);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-> Cập nhật thông tin sản phẩm thành công!");
            Console.ResetColor();
        }

        static void HandleDeleteProduct(ProductService s)
        {
            Console.WriteLine("=== XÓA SẢN PHẨM ===");
            string id = InputHelper.ReadNonEmptyString("Nhập mã sản phẩm cần xóa: ");
            if (s.DeleteProduct(id))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"-> Xóa thành công sản phẩm có mã '{id}'.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Không tìm thấy sản phẩm có mã '{id}' để xóa.");
                Console.ResetColor();
            }
        }

        static void HandleFindById(ProductService s)
        {
            string id = InputHelper.ReadNonEmptyString("Nhập mã sản phẩm cần tìm: ");
            var product = s.GetById(id);
            if (product != null)
            {
                PrintProductList(new[] { product }, "KẾT QUẢ TÌM KIẾM");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Không tìm thấy sản phẩm có mã '{id}'.");
                Console.ResetColor();
            }
        }

        static void HandleSearchByName(ProductService s)
        {
            string keyword = InputHelper.ReadNonEmptyString("Nhập từ khóa tìm kiếm trong tên: ");
            var results = s.SearchByName(keyword);
            PrintProductList(results, $"KẾT QUẢ TÌM KIẾM GẦN ĐÚNG VỚI TỪ KHÓA: '{keyword}'");
        }

        static void HandleFilterByCategory(ProductService s)
        {
            string category = InputHelper.ReadNonEmptyString("Nhập tên danh mục cần lọc: ");
            var results = s.FilterByCategory(category);
            PrintProductList(results, $"KẾT QUẢ LỌC THEO DANH MỤC: '{category}'");
        }

        static void HandleCategoryStatistics(ProductService s)
        {
            Console.WriteLine("=== THỐNG KÊ SẢN PHẨM THEO DANH MỤC ===");
            Console.WriteLine($"{"Danh mục",-18} | {"Số loại SP",10} | {"Tổng số lượng",13} | {"Tổng giá trị tồn",20}");
            Console.WriteLine(new string('-', 70));

            foreach (dynamic item in s.GetCategoryStatistics())
            {
                Console.WriteLine($"{item.Category,-18} | {item.ProductCount,10} | {item.TotalQuantity,13} | {item.TotalValue,18:N0} đ");
            }
        }

        static void HandleDisplayGroupedByCategory(ProductService s)
        {
            Console.WriteLine("=== DANH SÁCH SẢN PHẨM NHÓM THEO DANH MỤC (SortedDictionary) ===");
            var grouped = s.GetProductsGroupedByCategory();
            foreach (var group in grouped)
            {
                Console.WriteLine($"\n[Danh mục: {group.Key.ToUpper()}] (Tổng: {group.Value.Count} sản phẩm)");
                PrintHeader();
                foreach (var p in group.Value)
                {
                    Console.WriteLine(p);
                }
            }
        }

        static void HandleTagManagement(ProductService s)
        {
            while (true)
            {
                Console.WriteLine("\n--- MENU QUẢN LÝ TAG ---");
                Console.WriteLine("1. Hiển thị tất cả tag duy nhất (HashSet)");
                Console.WriteLine("2. Tìm sản phẩm theo tag");
                Console.WriteLine("3. Đếm số sản phẩm có tag");
                Console.WriteLine("0. Quay lại menu chính");
                Console.Write("Chọn: ");
                string? opt = Console.ReadLine()?.Trim();

                switch (opt)
                {
                    case "1":
                        var tags = s.GetAllTags();
                        Console.WriteLine("\nDanh sách toàn bộ các Tag: " + (tags.Count > 0 ? string.Join(", ", tags) : "(Trống)"));
                        break;
                    case "2":
                        string tagFind = InputHelper.ReadNonEmptyString("Nhập tag cần tra cứu: ");
                        var foundList = s.FindProductsByTag(tagFind);
                        PrintProductList(foundList, $"SẢN PHẨM CÓ CHỨA TAG '{tagFind}'");
                        break;
                    case "3":
                        string tagCount = InputHelper.ReadNonEmptyString("Nhập tag cần đếm: ");
                        int count = s.CountProductsByTag(tagCount);
                        Console.WriteLine($"-> Có {count} sản phẩm mang tag '{tagCount}'.");
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Tùy chọn không hợp lệ!");
                        break;
                }
            }
        }
    }
}
