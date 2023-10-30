# Homework 17
To work you need to create a DB with tables:
Users (user_id(int), login(varchar), pass(varchar), email(varchar), user_name(varchar), birth_date(date));
Posts (post_id(int), post_content(text), fk_user_id(foreign key to user id), post_date(date));
And add file app.json with db connection string
