# Toggl.Overtime

Very simple web application which uses AngularJS and the ASP.NET Web API to calculate the overtime with data from toggl.

The calculation of the overtime works as follows:
You enter your regular working hours per week. This time is divided by 5 (since a normal week has 5 working days).
The overtime is then calculated for single days (and not per work week!).
A day only counts as a workday if you have at least 1 second worked that day. 
