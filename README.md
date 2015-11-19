# Toggl.Overtime

Very simple web application which uses AngularJS and the ASP.NET Web API to calculate the overtime with data from toggl.

The calculation of the overtime works as follows:
You enter your regular working hours per week. This time is divided by 5 (since a normal week has 5 working days).
The overtime is then calculated for single days (and not per work week - this has the advantage,
that if you have had a day off you won't have to add "8 hours" or something like that, to not lose time).
A day therefore only counts as a workday if you have at least worked one second on that day.
Also if you have worked at least five days in a week, any worktime on the other two days counts as overtime.