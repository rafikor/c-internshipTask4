# Solution to task 4

This is a Web application with registration and authentication.
Non-authenticated users have no access to the user management (admin panel).
Authenticated users have access the user management table: id, name, e-mail, last login time, registration time, status (active/blocked).
The left column of the table contains checkboxes without labels for multiple selection (table header contains “Select All” checkbox without label).
There is a toolbar over the table with the flooring actions: Block (red button with text), Unblock (icon), Delete (icon).
Bootstrap is used as a CSS framework.
Every user is able to block or delete himself or any other user.
If user account is blocked or deleted any next user’s request redirects to the login page.
User can use any non-empty password (even one character).
Blocked user is not able to login, deleted user can re-register.

Actual connection string from server is not in repo (and not in the history)