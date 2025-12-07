-- CREATE TABLE USERS(username TEXT NOT NULL  PRIMARY KEY, password NOT NULL, email NOT NULL);

-- INSERT INTO USERS (username, password, email) 
-- VALUES ('JOE', 'JOEpassword', 'mike@yahoo.com');

UPDATE USERS SET email = 'notmike@gmail.com' 
WHERE username = 'mike';

-- SELECT email  FROM USERS
-- WHERE username like 'm%';

