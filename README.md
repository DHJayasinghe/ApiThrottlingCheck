# ApiThrottlingCheck

### What this repository is about?
 
There was an External API used in a critical part of a system, and faced degraded performance and failures on processing messages on large quantity. 
That External API seems to have throttling enabled which blocks requestes after X Amount of Calls on Y amount of Time. 

Due to the lacking support on API documentation or Developer contact to know these 2 factors (X and Y), needed a way to identify these 2 unknow factors so we can adjust the code or find an alternatice to address this problem. 
This project contains code to analyze and understand the Average response time of a Throttling Enabled API endpoint, 
and how it's change when doing API calls parallely with different batch sizes.


### How it works?

The attempt is to do Batches of REST API calls to Target API endpoint paralley as Iterations and record the response time. For example, on first iteration this code will do 5 API calls paralley and capture the average response time. On next iteration increase the batch size by 5 (10 api calls) and do the same. Keep continue the iterations until batch size hit to 30. After the batch size hit to 30, start reducing the batch size and keep continue iterations until it reaches to initial batch size (5). Finaly, check all response times on the Console to understand the Trend.
