import Button from 'react-bootstrap/Button';
import {useLocation} from 'react-router-dom';
import { useEffect, useState } from "react";
import queryString from 'query-string';
import axios from 'axios';
import Sunburst from 'sunburst-chart';

const ShooterData = () => {
  const location = useLocation();
  const search = location.search;
  let params = queryString.parse(search)
  const teamA = params['teamA'];
  const teamB = params['teamB'];
  const [rosterA, setRosterA] = useState([]);
  const [rosterB, setRosterB] = useState([]);
  const [rosterFull, setRosterFull] = useState([]);
  const [player0, setPlayer0] = useState([]);
  const [player1, setPlayer1] = useState([]);
  const [player2, setPlayer2] = useState([]);
  const [player3, setPlayer3] = useState([]);
  const [player4, setPlayer4] = useState([]);
  const [player5, setPlayer5] = useState([]);
  const [player6, setPlayer6] = useState([]);
  const [player7, setPlayer7] = useState([]);
  const [player8, setPlayer8] = useState([]);
  const [player9, setPlayer9] = useState([]);
  const [focusedPlayer, setFocusedPlayer] = useState([]);

  useEffect(() => {
    const fetchTeams = async () => {
      axios.defaults.headers.get['Access-Control-Allow-Origin'] = '*';
      const full = [];
      axios.defaults.headers.get['Content-Type'] = 'application/x-www-form-urlencoded';
      await axios.get(`https://localhost:5001/api/v1/players`, { params: {teamName: teamA}}).then((res) => {
        const roster = [];
        for (const player of res.data) {
          roster.push(player);
          full.push(player);
        }
        
        setRosterA(roster);
      });
      await axios.get(`https://localhost:5001/api/v1/players`, { params: {teamName: teamB}}).then((res) => {
        const roster = [];
        for (const player of res.data) {
          roster.push(player);
          full.push(player);
        }
        
        setRosterB(roster);
      });
      setRosterFull(full);
    };
    fetchTeams();
  }, []);

  function clearChart()
  {
    document.getElementById("chart").innerHTML = "";
  }

  function clearIndividualChart()
  {
    document.getElementById("individualChart").innerHTML = "";
  }

  //add all selections to a list based off of position in array
  //add button to search based off of contents of roster selections
  //this implementation will have a bug if the selected value doesn't change.

  const source = axios.CancelToken.source();
  const timeout = setTimeout(() => {
    source.cancel();
    // Timeout Logic
  }, 20000);

  const acquireShootingDataWrapped = async () => 
  {
    // fullRosterShootingData clear the array of shooting data
    const recentGamesCount = [1, 3, 10];
    recentGamesCount.forEach((element) =>
    {
      acquireShootingData(element);
    });
  }

  const acquireShootingData = async (element) =>
  {
    axios.defaults.headers.get['Access-Control-Allow-Origin'] = '*';
    axios.defaults.headers.get['Content-Type'] = 'application/x-www-form-urlencoded';
    await axios.get(`https://localhost:5001/api/v1/player/stats/gameAverages?playerIds=${player0}&playerIds=${player1}&playerIds=${player2}&playerIds=${player3}&playerIds=${player4}&playerIds=${player5}&playerIds=${player6}&playerIds=${player7}&playerIds=${player8}&playerIds=${player9}&numberOfRecentGames=${element}`, {cancelToken: source.token})
    .then((res) => {
      // setShooterData(res.data);
      const objectPasser = res.data;

      Sunburst()
      .data(formatForChart(objectPasser, element))
      .size('size')
      .color('color')
      .width(500)
      .height(500)
      .radiusScaleExponent(1)
      (document.getElementById('chart'));
    });
  }
  
  const acquireFocusedShootingDataWrapped = async () => 
  {
    // fullRosterShootingData clear the array of shooting data
    const recentGamesCount = [1, 3, 10];
    recentGamesCount.forEach((element) =>
    {
      acquireFocusedShootingData(element);
    });
  }

  const acquireFocusedShootingData = async (element) =>
  {
    axios.defaults.headers.get['Access-Control-Allow-Origin'] = '*';
    axios.defaults.headers.get['Content-Type'] = 'application/x-www-form-urlencoded';
    await axios.get(`https://localhost:5001/api/v1/player/${focusedPlayer}/stats/gameAverages?numberOfRecentGames=${element}`, {cancelToken: source.token})
    .then((res) => {
      // setShooterData(res.data);
      const objectPasser = res.data;

      Sunburst()
      .data(formatForChart(objectPasser, element))
      .size('size')
      .color('color')
      .width(500)
      .height(500)
      .radiusScaleExponent(1)
      (document.getElementById('individualChart'));
    });
  }

  const formatForChart = (shooterData, element) =>
  {
    console.log(shooterData);

    const totalShots = shooterData.fg2a + shooterData.fg3a;

    var chartMaker = {
      "name": `Shots Attempted: ${element} game history`, 
      "color": "#84bef7", "children": [{
          "name": `2s attempted: ${(shooterData.fg2a / totalShots).toFixed(2) * 100}% of all shots`,
          "color": "#e0b6d7",
          "children": [{"name": `2s made: ${shooterData.fg2_pct.toFixed(2) * 100}% of 2s attempted and ${(shooterData.fg2m / totalShots).toFixed(2) * 100}% of all shots`, "color": "#dc94cd", "size": shooterData.fg2m}, {"name": "2s missed", "color": "#e6d3e2", "size": shooterData.fg2a - shooterData.fg2m}]
      }, {
          "name": `3s attempted: ${(shooterData.fg3a / totalShots).toFixed(4) * 100}% of all shots`,
          "color": "rgb(25 135 84 / 91%)",
          "children": [{"name": `3s made: ${shooterData.fg3_pct.toFixed(4) * 100}% of 3s attempted and ${(shooterData.fg3m / totalShots).toFixed(2) * 100}% of all shots`, "color": "#20b76e", "size": shooterData.fg3m}, {"name": "3s missed", "color": "rgb(126 182 151)", "size": shooterData.fg3a - shooterData.fg3m}]
      }]
    };

    return chartMaker;
  }

  const handleAllClick = (e) => {
    clearChart();
    acquireShootingDataWrapped();
  }

  const handleFocusClick = (e) => {
    clearIndividualChart();
    acquireFocusedShootingDataWrapped();
  }

  //#region playerChange handlers


  const onPlayer0Change = (e) => {
    const playerSelected = e.target.value;
    setPlayer0(playerSelected);
  }

  const onPlayer1Change = (e) => {
    const playerSelected = e.target.value;
    setPlayer1(playerSelected);
  }

  const onPlayer2Change = (e) => {
    const playerSelected = e.target.value;
    setPlayer2(playerSelected);
  }

  const onPlayer3Change = (e) => {
    const playerSelected = e.target.value;
    setPlayer3(playerSelected);
  }

  const onPlayer4Change = (e) => {
    const playerSelected = e.target.value;
    setPlayer4(playerSelected);
  }

  const onPlayer5Change = (e) => {
    const playerSelected = e.target.value;
    setPlayer5(playerSelected);
  }

  const onPlayer6Change = (e) => {
    const playerSelected = e.target.value;
    setPlayer6(playerSelected);
  }

  const onPlayer7Change = (e) => {
    const playerSelected = e.target.value;
    setPlayer7(playerSelected);
  }

  const onPlayer8Change = (e) => {
    const playerSelected = e.target.value;
    setPlayer8(playerSelected);
  }

  const onPlayer9Change = (e) => {
    const playerSelected = e.target.value;
    setPlayer9(playerSelected);
  }

  const onFocusedPlayerChange = (e) => {
    const playerSelected = e.target.value;
    setFocusedPlayer(playerSelected);
  }

  //#endregion

  return ( 
    <div className="row mt-3">
    <div className="offset-md-2 col-md-4">
      Select Players:
    </div>
    <div className="col-md-4 mb-3">
      <select className="form-select" onChange={onPlayer0Change}>
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer1Change}>
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer2Change}>
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer3Change}>
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer4Change}>
        {rosterA.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
    <div className="text-center">
      <p></p>
      Versus
      <p></p>
    </div>
      <select className="form-select" onChange={onPlayer5Change}>
        {rosterB.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer6Change}>
        {rosterB.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer7Change}>
        {rosterB.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer8Change}>
        {rosterB.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>
      <select className="form-select" onChange={onPlayer9Change}>
      {rosterB.map((c) => (
        <option key={c.id} value={c.id}>
          {c.first_name} {c.last_name}
        </option>
      ))}
    </select>
      <p></p>
      <p></p>
      Focus On
      <p></p>
      <select className="form-select" onChange={onFocusedPlayerChange}>
        {rosterFull.map((c) => (
          <option key={c.id} value={c.id}>
            {c.first_name} {c.last_name}
          </option>
        ))}
      </select>  
    </div>
    <div className="text-center">
    <Button onClick={handleAllClick}>
        Get Full Shooting Stats
      </Button>
      <p></p>
    <Button onClick={handleFocusClick}>
        Get Focused Player Shooting Stats
      </Button>
    </div>
      <p>
      </p>
    <div className="offset-sm-1 col-md-4">
      Team Numbers
      <div id="chart"></div>
    </div>

    <div className="offset-sm-2 col-md-4 mb-3">
      Individual Numbers
      <div id="individualChart"></div>
    </div>
  </div>
  );
}

export default ShooterData;
