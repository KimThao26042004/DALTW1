using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products


        public ActionResult Index(int? page)
        {
            int pageSize = 6; // Số sản phẩm mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại, mặc định là trang 1

            var products = db.Products.OrderBy(p => p.Title).ToPagedList(pageNumber, pageSize);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = products.PageCount;
            return View(products);
        }

        public ActionResult Detail(string alias,int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                db.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            var countReview = db.Reviews.Where(x => x.ProductId == id).Count();
            ViewBag.CountReview = countReview;
            return View(item);
        }
        public ActionResult ProductCategory(string alias, int id, int? page)
        {
            int pageSize = 6; // Số sản phẩm mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại, mặc định là trang 1

            var items = db.Products.Where(x => id <= 0 || x.ProductCategoryId == id)
                                   .OrderBy(p => p.Title)
                                   .ToPagedList(pageNumber, pageSize);

            var cate = db.ProductCategories.Find(id);
            ViewBag.CateName = cate != null ? cate.Title : "Danh mục sản phẩm";
            ViewBag.CateId = id;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = items.PageCount;

            return View(items);
        }


        public ActionResult SaleProductCategory(string alias, int id, int? page)
        {
            int pageSize = 6; // Số sản phẩm mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại, mặc định là trang 1

            var items = db.Products.Where(x => x.IsSale && (id <= 0 || x.ProductCategoryId == id))
                                   .OrderBy(p => p.Title)
                                   .ToPagedList(pageNumber, pageSize);

            var cate = db.ProductCategories.FirstOrDefault(x => x.Id == id || x.Alias == alias);
            ViewBag.CateName = cate != null ? cate.Title : "Sản phẩm khuyến mãi";
            ViewBag.CateId = id;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = items.PageCount;

            return View(items);
        }


        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x => x.IsHome && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductSales()
        {
            var items = db.Products.Where(x => x.IsSale && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult ProductSales(int? page)
        {
            int pageSize = 6; // Số sản phẩm mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại, mặc định là trang 1

            var items = db.Products.Where(x => x.IsSale && x.IsActive)
                                   .OrderBy(p => p.Title)
                                   .ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = items.PageCount;

            return View(items);
        }



    }
}