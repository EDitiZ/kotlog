import './LandingPage.css';
import React from 'react';
import { Link } from 'react-router-dom';

const LandingPage = () => {
  return (
    <div className='landing-page'>
      <h1>Finance Manager</h1>
      <p>Your Personal Finance Management Solution</p>
      <div className='button-container'>
        <Link to='/login'>
          <button className='button'>Login</button>
        </Link>
        <Link to='/register'>
          <button className='button'>Register</button>
        </Link>
      </div>
    </div>
  );
};

export default LandingPage;
