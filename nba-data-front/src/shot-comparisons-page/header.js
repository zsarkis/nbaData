// import { Component } from 'react';
import logo from './logo.svg';

const Header = ({subtitle, title}) => (
  <header className="row">
    <div className="col-md-5">
      <img src={logo} className="logo" alt="logo"/>
    </div>
    <div className="col-md-7 mt-5 subtitle">
      {subtitle}
    </div>
  </header>
)

export default Header;
