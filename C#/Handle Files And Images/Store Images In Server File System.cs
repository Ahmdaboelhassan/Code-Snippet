// Store Image
  var files = Request.Form.Files;
  IFormFile image = files.FirstOrDefault();
  
  string randomName = Guid.NewGuid().ToString();
  string extension = Path.GetExtension(image.FileName);
  string fullPath = Path.Combine(_wwwRoot.WebRootPath, "Images", "Mobile", randomName + extension);

  using(var file = new FileStream(fullPath , FileMode.Create))
  {
      image.CopyTo(file);
  }
  mobile.photoURL = Path.Combine("\\Images", "Mobile", randomName + extension); 




// For Reading  
// simple Case 
// <img src="@(Model.Mobile.id == 0 ? String.Empty: Model.Mobile.photoURL )" alt="Mobile Photo" id="img" class="w-100" />
 
 // When It Outside Server Directory 
    if (vm.ImagePath != null && !string.IsNullOrEmpty(atttachmentPath))
   {
       System.Drawing.Image image = System.Drawing.Image.FromFile(Path.Combine(atttachmentPath, vm.ImagePath));

       using (var memoryStream = new MemoryStream())
       {
           image.Save(memoryStream, ImageFormat.Png);
           var Image = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
           vm.ImagePath = Image;
       }

   }
 //<img src="@(Model.ImagePath == null ? String.Empty: Model.ImagePath)" alt="Mobile Photo" id="img" class="w-100" />