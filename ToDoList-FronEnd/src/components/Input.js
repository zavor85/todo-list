import React from 'react'
import '../App.css'

const Input = ({ newInputToDo, handleToDoChange, handleOnSubmit, newToDo }) => {
  return (
    <form onSubmit={handleOnSubmit}>
      <div className="todo-input">
        <input
          value={newInputToDo}
          placeholder='New Todo'
          onChange={handleToDoChange}
          autoFocus
          required
        />
        <button type="submit">Add</button>
      </div>
      {newToDo}
    </form>
  )
}

export default Input