
$(document).ready(function () {
    $(document).on("click", ".imageUpload", function () {
        var file = $(this).data("fileid");
        $("#" + file).click();
    });
    $(document).on("change", ".file", function () {
        
        var thi$ = $(this);
        var ext = $(thi$).data("ext");
        var file, img, width, height, size;
        if ((file = this.files[0])) {
            var fileName = file.name;
            var fileExt = '.' + fileName.split('.').pop();
            var regex = new RegExp("(" + ext + ")$");
            if (regex.test(fileExt)) {
                img = new Image();
                var reader = new FileReader();
                //Read the contents of Image File.
                reader.readAsDataURL(file);
                reader.onload = function (e) {

                    if (ext == ".mp3") {
                        $('#' + $(thi$).data("id")).val(e.target.result);
                        var audio = $('#' + $(thi$).data("img")).parent();
                        $('#' + $(thi$).data("img")).attr("src", e.target.result);
                        audio[0].pause();
                        audio[0].load();//suspends and restores all audio element
                    }
                    else {
                        var image = new Image();
                        //Set the Base64 string return from FileReader as source.
                        image.src = e.target.result;
                        //Validate the File Height and Width.
                        image.onload = function () {
                            width = this.width;
                            height = this.height;
                            size = file.size;
                            //if (width <= 100 && height <= 100 && size <= 100000) {
                            $('#' + $(thi$).data("id")).val(image.src);
                            $('#' + $(thi$).data("img")).attr("src", image.src);
                            return true;
                            //}
                            //else {
                            //    alert("Height and Width must not exceed 100px. and it should be less than 100kb i.e (100x100),Upload image that has valid height,width and size.")
                            //    return false;
                            //}
                        }
                    };
                }
                //reader.readAsDataURL(file);
            }
            else {
                alert("Please select a valid Image file.");
            }
        }
    });


});