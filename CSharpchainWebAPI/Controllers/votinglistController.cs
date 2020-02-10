using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;

namespace CSharpchainWebAPI.Controllers
{
    public class VotinglistController : BaseController
    {
        // GET: votinglist
        public ActionResult Index()
        {
            DotBauCu dbc = new DotBauCu();
            ViewBag.title = "Danh sách đợt bỏ phiếu";
            ViewBag.list_dbc = dbc.get_dotbaucu();
            return View();
        }

        // GET: votinglist/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: votinglist/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: votinglist/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: votinglist/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: votinglist/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: votinglist/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: votinglist/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
