   $(document).ready(()=>{
        $("#imageInput").on("change", function () {

            var imageUrl = window.URL.createObjectURL(this.files[0]);
                document.getElementById("img").setAttribute("src", imageUrl)
            document.getElementById("imageholder").classList.remove("d-none"); 
        })
   })