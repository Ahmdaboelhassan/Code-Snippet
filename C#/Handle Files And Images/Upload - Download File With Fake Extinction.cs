    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UploadFiles(UploadFilesFormViewModel model)
    {
        //Validate files extenstions and size

        List<UploadedFile> uploadedFiles = new();

        foreach (var file in model.Files)
        {
            var fakeFileName = Path.GetRandomFileName();

            UploadedFile uploadedFile = new()
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                StoredFileName = fakeFileName
            };

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fakeFileName);

            using FileStream fileStream = new(path, FileMode.Create);
            file.CopyTo(fileStream);

            uploadedFiles.Add(uploadedFile);
        }

        _context.AddRange(uploadedFiles);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult DownloadFile(string fileName)
    {
        var uploadedFile = _context.UploadedFiles.SingleOrDefault(f => f.StoredFileName == fileName);

        if (uploadedFile is null)
            return NotFound();

        var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

        MemoryStream memoryStream = new();
        using FileStream fileStream = new(path, FileMode.Open);
        fileStream.CopyTo(memoryStream);

        memoryStream.Position = 0;

        return File(memoryStream, uploadedFile.ContentType, uploadedFile.FileName);
    }
    [HttpGet]
		public IActionResult DownloadFileV2(string fileName)
		{
		    var uploadedFile = _context.UploadedFiles.SingleOrDefault(f => f.StoredFileName == fileName);
		
		    if (uploadedFile is null)
		        return NotFound();
		
		    var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
		
		    if (!System.IO.File.Exists(path))
		        return NotFound();  // If the physical file doesn't exist
		
		    FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
		
		    return new FileStreamResult(fileStream, uploadedFile.ContentType)
		    {
		        FileDownloadName = uploadedFile.FileName
		    };
		}