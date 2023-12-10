import React from 'react'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { useSelector } from 'react-redux'
import { Navigate } from 'react-router-dom'
import Login from './components/Login'
import Register from './components/Register'
import LandingPage from './pages/LandingPage'
import HomePage from './pages/HomePage'
import { useDispatch } from 'react-redux'
import { login } from './store'
import AddTransaction from './components/AddTransaction'
import AddBudget from './components/AddBudget'

const App = () => {
    const dispatch = useDispatch();
    const user = JSON.parse(localStorage.getItem("user"));
    if(user) dispatch(login(user))
   
  return (
    <BrowserRouter>
    <Routes>
        <Route path='/'>
           <Route index element={<LandingPage/>}/>
            <Route path='/login' element={<Login/>}/>
            <Route path='/register' element={<Register/>}/>
            <Route path='/home' element={<HomePage/>}/>
            <Route path='/add-transaction' element={<AddTransaction/>}/>
            <Route path='/add-budget' element={<AddBudget/>}/>
        </Route>
    </Routes>
    </BrowserRouter>
  )
}

export default App