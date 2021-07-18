import './team-select-page.css';
import Header from './header';
import { useEffect, useState } from "react";
import {BrowserRouter as Router, Route} from "react-router-dom"
import Matchup from './matchup-select';
import ShooterData from '../shot-comparisons-page/shooter-data';
import axios from 'axios';

function App() {
  //read useEffect docs
  //call hooks at the top level (not conditional or in loops)
  const [allTeams, setAllTeams] = useState([]);

  useEffect(() => {
    const fetchTeams = async () => {
      await axios.get("https://www.balldontlie.io/api/v1/teams").then((res) => {
        const teamList = [];
        for (const team of res.data.data) {
          teamList.push(team.full_name);
        }
        
        setAllTeams(teamList);
      });
    };
    fetchTeams();
  }, []);

  return (
    <Router>
      {/* TODO: figure out how to throw on incorrect path */}
      <Route exact path="/">
        <div className="container">
          <Header subtitle = "Providing shooting metrics by matchup" />
          <Matchup allTeams={allTeams}/>
        </div>
      </Route>
      <Route path="/matchup">
        <div className="container">
          <Header subtitle = "Providing shooting metrics by matchup" />
          <ShooterData></ShooterData>
        </div>
      </Route>
    </Router>
  );
}

export default App;
