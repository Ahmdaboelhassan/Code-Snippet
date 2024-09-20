private bool Checkimage(IFormFile image , ProcessorViewModel processorViewModel)
    {
        //check Extistion and Length
        List<string> AllowedExtinstions = new List<string> { ".png", ".jpg" };
        if (!AllowedExtinstions.Contains(Path.GetExtension(image.FileName.ToLower())))
        {
            ModelState.AddModelError("image", "Extistion Not Allowed!!");
            return false;
        }
        if (image.Length > 2_097_152)
        {
            ModelState.AddModelError("image", "Length Not Allowed!! , The Max Length Is 2MB");
            return false;
        }
        return true;
        
    }