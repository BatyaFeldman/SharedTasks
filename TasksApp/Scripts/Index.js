$(() => {
    const hub = $.connection.indexHub;

    $.connection.hub.start().done(()=> {
        hub.server.getAll();
    });



    $("#addBtn").on('click', function () {

        const taskTitle = $("#task").val();
        hub.server.addTask(taskTitle);
        $("#task").val(' ');
        

    });

   
    hub.client.getTasks = tasks => {
        $("table tr:gt(0)").remove();
        const userId = $("#userId").val();
            tasks.forEach(task=> {
                let btn;
                if (task.UserId === null) {
                    btn = `<button class="btn btn-primary" data-userid=${userId} data-taskid=${task.Id}>I am doing this one</button>`;
                } else if (task.UserId != userId) {
                    btn = `<button class="btn btn-success" data-userid=${userId} data-taskid=${task.Id} disabled>${task.UserDoingIt} is doing this</button>`;
                } else {
                   btn= `<button class="btn btn-danger" data-userid=${userId} data-taskid=${task.Id}>I am done!</button>`
                }
                $("#thisTable").append(`<tr><td>${task.Title}</td><td>${btn}</td></tr>`);
            });        
    }

    $('body').on('click', ".btn-primary", function () {
        const userId2 = $(this).data('userid');
        const taskId = $(this).data('taskid');
        hub.server.updateTaskWithUser(taskId, userId2)
       
    });

    $('body').on('click', ".btn-danger", function () {
        const taskId = $(this).data('taskid');
        hub.server.completeTask(taskId);

    });

});