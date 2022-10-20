const inputField = document.getElementsByClassName("input-box-txt")[0];
const btn = document.getElementsByClassName("input-box-cancel")[0];
var firstReplyAppearance = true;

function ReplyToComment(index) {
    if (firstReplyAppearance) {
        document.getElementsByClassName('comment-reply')[index].innerHTML = "Cancel";
        document.getElementsByClassName('comments-section')[0].setAttribute("style", `height: ${document.getElementsByClassName('comments-section')[0].clientHeight + 100}px;`);
        document.getElementsByClassName('comments-center')[0].setAttribute("style", `height: ${document.getElementsByClassName('comments-center')[0].clientHeight + 100}px;`);
        document.getElementsByClassName('comment-form')[index+1].style.display = "block";
        firstReplyAppearance = false;
    }
    else {
        document.getElementsByClassName('comment-reply')[index].innerHTML = "Comment";
        document.getElementsByClassName('comments-section')[0].setAttribute("style", `height: ${document.getElementsByClassName('comments-section')[0].clientHeight - 100}px;`);
        document.getElementsByClassName('comments-center')[0].setAttribute("style", `height: ${document.getElementsByClassName('comments-center')[0].clientHeight - 100}px;`);
        document.getElementsByClassName('comment-form')[index+1].style.display = "none";
        firstReplyAppearance = true;
    }
}
btn.addEventListener('click', () => {
    inputField.value = "";
});