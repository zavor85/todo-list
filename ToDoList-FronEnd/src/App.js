import React, { useState, useEffect } from 'react';
import './App.css';
import userService from './axios/User';
import ToDo from './components/ToDo'
import Input from './components/Input'


const App = () => {
  const [toDos, setToDos] = useState([])
  const [newToDo, setNewToDo] = useState('')

  const getHook = () => {
    userService
      .getAll()
      .then(response => {
        setToDos(response.data)
      })
      .then(() => updateInfo())
      .catch(error => {
        console.log(error.name)
      })

  }

  useEffect(getHook, [])

  const removeToDo = (id) => {
    const findToDo = toDos.filter(todo => todo.id === id)[0]
    if (window.confirm(`Are you sure you want to delete ${findToDo.toDoTitle}`)) {
      userService
        .remove(id)
        .then(() => updateInfo())
        .catch(error =>
          console.log(error.name)
        )
    }
  }

  const removeAllToDo = () => {
    if (window.confirm(`Are you sure you want to delete All To Dos`)) {
      userService
        .removeAll()
        .then(() => updateInfo())
        .catch(error => console.log(error.name))
    }
  }

  const updateInfo = () => {
    userService
      .getAll()
      .then(response => {
        setToDos(response.data)
      })
      .catch(error =>
        console.log(error.name))
  }

  const handleToDoChange = (event) => {
    setNewToDo(event.target.value)
  }

  const handleOnSubmit = () => {
    const newInputToDo =
    {
      toDoTitle: newToDo
    }

    userService
      .create(newInputToDo)
      .then(response => response.data)
      .then(returnedComment => {
        setToDos(toDos.concat(returnedComment))
      })
      .then(() => updateInfo())
      .catch(error =>
        console.log(error.name))
  }

  const updateToDo = (id) => {
    const findToDo = toDos.filter(todo => todo.id === id)[0]

    let toDoForChange = {}
    if (findToDo.isCompleted === false) {
      toDoForChange = {
        isCompleted: true
      }
    }
    else if (findToDo.isCompleted === true) {
      toDoForChange = {
        isCompleted: false
      }
    }

    userService
      .update(id, toDoForChange)
      .then(() => updateInfo())
      .catch(error =>
        console.log(error.name))
  }


  return (
    <div>
      <div className="todo-main">
        <div className="todo-header">
          <h1>To Do List</h1>
          <Input
            newInputToDo={newToDo}
            handleOnSubmit={handleOnSubmit}
            handleToDoChange={handleToDoChange}
          />
        </div>
        <ul className="todo-container">
          {toDos.map(todo => (
            <ToDo
              key={todo.id}
              todo={todo}
              handleCheckBoxChange={() => updateToDo(todo.id)}
              remove={() => removeToDo(todo.id)}
            />
          ))}
        </ul>
      </div>
      <div className="delete-all-btn">
        <button onClick={removeAllToDo}>Delete All</button>
      </div>
    </div>
  )
}

export default App;
