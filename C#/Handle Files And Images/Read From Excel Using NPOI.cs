   public async Task<IActionResult> ImportExcelFileV2(IFormFile FormFile)
   {
       if (FormFile == null)
           return RedirectToAction(nameof(Index));

       //get file name
       var filename = ContentDispositionHeaderValue.Parse(FormFile.ContentDisposition).FileName.Trim('"');

       //get extension
       string extension = Path.GetExtension(filename).ToLower();

       if (extension != ".xlsx" && extension != ".xls")
           return RedirectToAction(nameof(Index));


       //get path
       var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
       //create directory "Uploads" if it doesn't exists
       if (!Directory.Exists(MainPath))
       {
           Directory.CreateDirectory(MainPath);
       }

       //get file path 
       var filePath = Path.Combine(MainPath, FormFile.FileName);

       using (System.IO.Stream stream = new FileStream(filePath, FileMode.Create)) // create the file 
       {
           await FormFile.CopyToAsync(stream);
       }


       var userId = HttpContext.Session.GetInt32("CurrentUserId");
       var user = UserVM.GetUser(userId.Value, bussinseContext);
       int? BID = user.Branch_Id;
       int CID = user.Company_Id;
       //Create Data fron sheet
       var code = Helper.GetNextItemCode(CID, BID, bussinseContext); //insert code
       var unit = bussinseContext.IC_UOMBL.GetWithInclude(x => x.Name_Ar == "وحدة").FirstOrDefault().Id;// get default unit for item 

       List<IC_ItemVM> lst = new List<IC_ItemVM>();

       using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
       {

           IWorkbook workbook;
           if (extension == ".xls")
           {
               workbook = new HSSFWorkbook(fileStream);
           }
           else
           {
               workbook = new XSSFWorkbook(fileStream);
           }

           // Select the first worksheet
           ISheet worksheet = workbook.GetSheetAt(0);
   
           int rowIndex = 0;
           // Loop through the rows used in the worksheet 
           foreach (IRow row in worksheet)
           {
               if (rowIndex == 0)
               {
                   rowIndex++;
                   continue; // skip first row (Header Row)
               }

               if (!string.IsNullOrEmpty(row.GetCell(0).GetTrimmedText())) { 
                   //item data 
                   IC_ItemVM i = new IC_ItemVM();

                   i.Item_Code1 = code;
                   i.Item_Code2 = row.GetCell(1).GetTrimmedText();
                   i.Description = row.GetCell(0).GetTrimmedText();
                   i.DescriptionEn = row.GetCell(0).GetTrimmedText();
                   i.Item_NameAr = row.GetCell(2).GetTrimmedText();
                   i.Item_NameEn = string.IsNullOrEmpty(row.GetCell(3).GetTrimmedText()) ? row.GetCell(2).GetTrimmedText() : row.GetCell(3).GetTrimmedText();
                   i.Company_Id = CID;
                   i.Branch_Id = BID.Value;
                   if (bussinseContext.IC_CategoryBL.GetWithInclude(x => x.Name_Ar == row.GetCell(4).GetTrimmedText()).FirstOrDefault() == null)
                   {
                       OptimumERPModel.IC_Category category = new OptimumERPModel.IC_Category();
                       category.Name_Ar = category.Name_En = row.GetCell(4).GetTrimmedText();
                       category.ParentId = null;// change
                       category.UserId = 1;
                       category.BranchId = 1;
                       category.CompanyId = 1;
                       category.CreateDate = DateTime.Now;
                       category.CatNumber = bussinseContext.IC_CategoryBL.GetCode(1, 1);

                       bussinseContext.UnitOfWork.IC_CategoryRep.Add(category);
                       bussinseContext.UnitOfWork.Complete();
                       i.Category_Id = category.Id;

                   }
                   code++;
               }
           }
           var res = IC_ItemVM.SaveExcelSheetV2(lst, bussinseContext, userId, BID.Value);




// Extistion Method To Get Value
 public static class NPOIExtensions
 {
     public static string GetTrimmedText(this ICell cell)
     {
         if (cell == null)
             return string.Empty;

         switch (cell.CellType)
         {
             case CellType.String:
                 return cell.StringCellValue.Trim(); ;

             case CellType.Numeric:
                 if (DateUtil.IsCellDateFormatted(cell))
                 {
                     return cell.DateCellValue.ToString().Trim();
                 }
                 return cell.NumericCellValue.ToString().Trim();

             case CellType.Boolean:
                 return cell.BooleanCellValue.ToString().Trim();

             case CellType.Formula:
                 return cell.CellFormula;

             case CellType.Blank:
                 return string.Empty;

             default:
                 return cell.ToString();

         }
     }
 }