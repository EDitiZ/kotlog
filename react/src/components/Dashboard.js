import React, { useState, useEffect } from "react";
import { Link, useParams } from "react-router-dom";
import axios from "axios";
import { useSelector } from "react-redux";
import "./Dashboard.css"; 

const Dashboard = () => {
  const { userName } = useSelector((state) => state.user.user);
  const [budgetGoals, setBudgetGoals] = useState([]);
  const [transactions, setTransactions] = useState([]);

  useEffect(() => {
    // Fetch the budget goals
    axios
      .get(`http://localhost:5083/api/finances/all-budgets?user=${userName}`)
      .then((response) => setBudgetGoals(response?.data?.result))
      .catch((error) => console.error("Error fetching budget goals:", error));

    // Fetch the transactions
    axios
      .get(`http://localhost:5083/api/finances/all-transactions/${userName}`)
      .then((response) => setTransactions(response?.data.result))
      .catch((error) => console.error("Error fetching transactions:", error));
  }, [userName]);

  return (
    <div className="dashboard-container">
      <h2>Welcome, {userName}!</h2>
      <div className="add-butonat">
        <Link to="/add-transaction" className="add-transaction-link">
          Add Transaction
        </Link>
        <Link to="/add-budget" className="add-transaction-link">
          Add Budget
        </Link>
      </div>

      {/* Display the Budget Goals */}
      <div className="budget-goals-container">
        <h3>Your Budget Goals</h3>
        <div className="budget-goals-list">
          <div className="bugdet-goal">
            {budgetGoals.length===0 ?<p>No budget goals set!</p> :budgetGoals.map((goal, index) => (
              <div key={index} className="budget-goal-item">
                <p>
                  {goal.category}: {goal.budget}
                </p>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Display the Transactions */}
      <div className="transactions-container">
        <h3>Your Latest Transactions</h3>
        {transactions.map((transaction, index) => (
          <div key={index} className="transaction-item">
            <p>
              <b>{transaction.amount}</b> DEN spent on{" "}
              <i>{transaction.category}</i> on {transaction.date}
            </p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Dashboard;
