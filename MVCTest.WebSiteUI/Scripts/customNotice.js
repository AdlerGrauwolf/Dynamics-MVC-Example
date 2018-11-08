var timer = null;

function customNotice(message, title, type, time){ 

var className = customeNoticeType(type);
    if (title == null || title == undefined){
    	title = "Notificación: ";
    }

    if(time == "" || time == null || time == undefined){
        time = 0;
    }

var id = $("#customAlertDiv").attr("id");
    
var customDiv = "<div class='customNotice notice " + className + " animated slideInDown' id='customAlertDiv'>";
customDiv += "<strong> " + title + " </strong>";
customDiv += message;      
customDiv += "<div class='notice-close' onclick='closeCustomNotice()'>"
customDiv += "&#x2716;"
customDiv += "</div>"
customDiv += "</div>";    

if(id != undefined){
    $("#customAlertDiv").remove();
}

$("body").prepend(customDiv);

if( time > 0 ){
    timer = new NoticeTimer(time);
    timer.startTimer();
}
}


function customeNoticeType(type) {

    var className;

    switch (type) {
        case "default":
            className = 'notice-default';           
            break;
        case "primary":
            className = 'notice-primary';
            break;
        case "error":
            className = 'notice-danger';
            break;
        case "success":
            className = 'notice-success';
            break;
        case "info":
            className = 'notice-info';
            break;
        case "warning":
            className = 'notice-warning';
            break;
        default:
            className = 'notice-default';
            break;
    }

    return className;
}



function closeCustomNotice(){    
	var customAlert = $("#customAlertDiv");
	if(customAlert.hasClass("slideInDown")){
		customAlert.removeClass("slideInDown").addClass( "slideOutUp" );
	}
    timer.resetTimer();        
}


// Timer

    function NoticeTimer(initialTime){
        this.initial = initialTime;
        this.count = initialTime;
        this.counter;
        this.initialMillis;

        var self = this;

        this.timer = function(){
            var current = Date.now();        
            self.count = self.count - (current - self.initialMillis);                      
            self.initialMillis = current;  

            if (self.count <= 0) {
                self.stopTimer();            
                self.resetTimer();
                closeCustomNotice();
            }                      
        }

        this.startTimer = function(){
            this.stopTimer();
            this.initialMillis = Date.now();
            this.counter = setInterval(this.timer, 1);   
        }

        this.resetTimer = function(){
            this.stopTimer();
            this.count = this.initial;
        }

        this.stopTimer = function(){
            clearInterval(this.counter);
        }

        this.timerValue = function(){
            var res = this.count / 1000;
            var time = res.toPrecision(this.count.length);
            time = time > '0' ? time : '0'; 
            return time;
        }
    }