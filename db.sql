DROP DATABASE IF EXISTS netflixnet;
CREATE DATABASE netflixnet CHARACTER SET utf8;
USE netflixnet;

GRANT ALL ON netflixnet.* TO 'netflix_user'@'localhost';
