import axios from 'axios'
const baseUrl = 'http://localhost:7000/todo-api'

const getAll = () => {
  return axios.get(baseUrl)
}

const create = newToDo => {
  return axios.post(baseUrl, newToDo)
}

const update = (id, newToDo) => {
  return axios.put(`${baseUrl}/${id}`, newToDo)
}

const remove = (id) => {
  return axios.delete(`${baseUrl}/${id}`)
}

const removeAll = () => {
  return axios.delete(`${baseUrl}`)
}


export default { getAll, create, update, remove, removeAll }

