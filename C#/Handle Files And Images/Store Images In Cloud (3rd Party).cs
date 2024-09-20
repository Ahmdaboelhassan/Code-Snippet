private Cloudinary _cloudinary;

    public PhotoService(IOptions<CloundinarySettings> CloudinarySettings){
         
       var _cloudinarySettings = CloudinarySettings.Value;
       var  _cloudinaryAccount = new Account(
            _cloudinarySettings.CloudName,
            _cloudinarySettings.APIKey,
            _cloudinarySettings.APISercert
        );

        _cloudinary = new Cloudinary(_cloudinaryAccount);
    }

    public async Task<string?> AddProfilePicture(int id, IFormFile? picture)
    {
        if (picture is not null && picture.Length > 0){
             using var stream = picture.OpenReadStream();

            var uploadParams = new ImageUploadParams{
                File = new FileDescription(id.ToString() , stream),
                Transformation = new Transformation()
                            .Height(500).Width(500).Gravity("face").Crop("fill"),
                Folder = "Blessmate/"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            if (result.StatusCode  != HttpStatusCode.OK)
                return null;

            return result.SecureUrl.AbsoluteUri;
        }
        return null; 
    }