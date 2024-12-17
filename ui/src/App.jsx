// src/App.js
import MealGrid from "./components/MealGrid";
import RestaurantGrid from "./components/RestaurantGrid";

function App() {
  return (
    <div style={{ padding: "20px" }}>
      <MealGrid />
      <RestaurantGrid />
    </div>
  );
}

export default App;
