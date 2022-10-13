# ApiThrottlingCheck

### What this repository is about?
 
There was an External API used in a critical part of a system, and faced degraded performance and failures on processing messages on large quantity. 
That External API seems to have throttling enabled which blocks requestes after X Amount of Calls on Y amount of Time. Due to lacking support on API documentation
or Developer contact to know these 2 factors (X and Y), needed a way to identify these 2 unknow factors so we can adjust the code or find an alternatice to address this problem. 
This project contains code to analyze and understand the Average response time of a Throttling Enabled API endpoint, 
and how it's change when doing API calls parallely with different batch sizes.
