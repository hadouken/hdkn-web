﻿using System.Web.Mvc;
using Peach.Core;
using Peach.Data;
using Peach.Data.Domain;
using Peach.Web.Models;

namespace Peach.Web.Controllers
{
    public class DownloadController : PeachController
    {
        private readonly IReleaseRepository _releaseRepository;

        public DownloadController(IConfiguration configuration, IUserRepository userRepository, IReleaseRepository releaseRepository)
            : base(configuration, userRepository)
        {
            _releaseRepository = releaseRepository;
        }

        public ActionResult Index()
        {
            var release = _releaseRepository.GetLatest();
            return View(release);
        }

        [Authorize(Roles = Role.Administrator)]
        public ActionResult New()
        {
            return View(new NewReleaseDto());
        }

        [Authorize(Roles = Role.Administrator)]
        [HttpPost]
        public ActionResult New(NewReleaseDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var release = new Release
            {
                DownloadUri = dto.DownloadUri,
                Version = dto.Version
            };

            _releaseRepository.Insert(release);

            return RedirectToAction("Index");
        }
	}
}