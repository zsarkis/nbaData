import {useHistory} from 'react-router-dom';
import Button from 'react-bootstrap/Button';
import 'bootstrap/dist/css/bootstrap.min.css';

const Matchup = ({teamA, teamB}) => {
  const history = useHistory();
  const matchup = teamA && teamB ? [teamA, teamB] : [];

  function handleClick(e) {
    console.log(teamA);
  }

  return ( 
    <div className="row mt-3">
    <div className="text-center">
      <Button onClick={handleClick}>
        Button Text
      </Button>
      {teamA}
    </div>
  </div>
  );
}

export default Matchup;
