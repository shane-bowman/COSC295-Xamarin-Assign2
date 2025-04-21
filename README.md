CWEB Pokémon Web Application
Overview
This Pokémon Web Application was developed for CWEB280 at Saskatchewan Polytechnic as part of our coursework by Shane Bowman and Zach Linzmeyer. It’s a web-based project that allows users to explore Pokémon data, leveraging the PokéAPI for dynamic content. The app includes features like user authentication, session tracking, and form autofill, showcasing our skills in web development and API integration.
Features

Pokémon Data Fetching: Retrieve and display Pokémon lists and detailed attributes (e.g., types, stats) using the PokéAPI.
Type Filtering: Filter Pokémon by type for a tailored browsing experience.
Google Authentication: Secure user login via Google OAuth using Passport.js.
Session-Based Activity Tracking: Log recent user activities (e.g., viewed Pokémon) with session management.
Cookie-Based Form Autofill: Enhance user experience by auto-filling forms based on previous inputs.
Image Upload: Allow users to upload images, integrated with session data.

Setup Instructions
This project is a Node.js web application hosted on GitHub at https://github.com/shane-bowman/CWEB280-Project1-2. Follow these steps to run it locally:
Prerequisites

Node.js: Version 14.x or higher (includes npm).
Git: To clone the repository.
A Google OAuth 2.0 client ID for authentication (set up via Google Cloud Console).

Steps

Clone the Repository:git clone https://github.com/shane-bowman/CWEB280-Project1-2.git
cd CWEB280-Project1-2


Install Dependencies:npm install

This installs required packages, including express, passport, passport-google-oauth20, and express-session.
Configure Environment Variables:
Create a .env file in the project root.
Add your Google OAuth credentials and session secret:GOOGLE_CLIENT_ID=your-google-client-id
GOOGLE_CLIENT_SECRET=your-google-client-secret
SESSION_SECRET=your-session-secret




Run the Application:node app.js


The app will run on http://localhost:3000 (or the port specified in your code).


Access the App:
Open http://localhost:3000 in your browser.
Log in with Google to explore Pokémon data and features.



Dependencies

Node.js: JavaScript runtime for server-side logic.
Express: Web framework for Node.js.
PokéAPI: External API for Pokémon data (pokeapi.co).
Passport.js: Authentication middleware for Google OAuth.
Express-Session: Session management for activity tracking.
dotenv: For environment variable management.

Academic Integrity
This project is the original work of Shane Bowman and Zach Linzmeyer, created for CWEB280 at Saskatchewan Polytechnic. It has been submitted as part of Shane Bowman’s TCOM 291 portfolio in compliance with Policy 1211(a) on academic integrity. All code and content are our own, with external libraries and APIs (e.g., PokéAPI, Passport.js) used as noted.
Reflection
Working on this project with Zach Linzmeyer improved my JavaScript skills, particularly in API integration with PokéAPI. Our collaboration on resolving Passport authentication issues honed my communication skills, ensuring seamless teamwork under tight deadlines. This experience built my resilience and taught me the value of debugging as a team. In the future, I plan to incorporate WebSockets for real-time features, applying the collaboration skills we developed.
Skills Demonstrated

JavaScript and Node.js for server-side development.
API integration with PokéAPI.
User authentication with Passport.js and Google OAuth.
Session management and cookie handling.
UI/UX design for an intuitive user experience.

Last Updated
April 21, 2025
