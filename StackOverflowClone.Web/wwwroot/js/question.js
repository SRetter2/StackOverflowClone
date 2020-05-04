$(() => {
    

    $("#save-btn").on('click', () => {
        const UserId = $("userId").val();
        const Text = $("text").text();
        const QuestionId = $("questionId").val();
        
        $.post(`/home/answer?UserId=${UserId}Text=${Text}QuestionId=${QuestionId}`, function (answer) {
        });
    });

    $("#like-btn").on('click', function() {
        const userId = $(this).data('user-id');
        console.log(userId);
        const questionId = $(this).data('question-id');
        $.post(`/home/likeQuestion?userId=${userId}questionId=${questionId}`, function (like) {
            $("#like-btn").prop('disabled', true);
        });
    });
});