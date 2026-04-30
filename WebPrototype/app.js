const i18n = {
  ru: {
    title: 'Token Agent — Браузерный прототип',
    lang: 'Язык:',
    mission: 'Миссия',
    stations: 'Станции',
    stickers: 'Доска стикеров',
    assemble: 'Собрать ответ (-10)',
    choose: 'Выберите ответ',
    notEnough: 'Недостаточно данных',
    energy: (cur, max) => `Энергия: ${cur} / ${max}`,
    actionCost: id => `${id} (-5)`,
    outOfPower: 'Энергия закончилась.',
    success: 'УСПЕХ',
    fail: 'ПРОВАЛ',
    correct: 'Ответ верный.',
    wrong: 'Ответ неверный.',
    missingGood: 'Верно: данных недостаточно.',
    missingBad: 'Ошибка: данных уже было достаточно.',
    spent: n => `Потрачено энергии: ${n}`,
    rank: r => `Ранг: ${r}`,
    next: 'Следующая миссия',
    restart: 'Повторить миссию',
    noMissions: 'Миссии не найдены',
  },
  en: {
    title: 'Token Agent — Browser Prototype',
    lang: 'Language:',
    mission: 'Mission',
    stations: 'Stations',
    stickers: 'Sticker board',
    assemble: 'Assemble answer (-10)',
    choose: 'Choose answer',
    notEnough: 'Not enough data',
    energy: (cur, max) => `Energy: ${cur} / ${max}`,
    actionCost: id => `${id} (-5)`,
    outOfPower: 'Out of power.',
    success: 'SUCCESS',
    fail: 'FAIL',
    correct: 'Correct answer.',
    wrong: 'Wrong answer.',
    missingGood: 'Correct: not enough data.',
    missingBad: 'Incorrect: data was enough.',
    spent: n => `Spent energy: ${n}`,
    rank: r => `Rank: ${r}`,
    next: 'Next mission',
    restart: 'Restart mission',
    noMissions: 'No missions found',
  }
};

const state = { missions: [], missionIndex: 0, mission: null, maxEnergy: 0, energy: 0, stickers: new Map(), lang: 'ru' };
const ACTION_COST = 5;
const ASSEMBLE_COST = 10;

const t = () => i18n[state.lang];

async function loadMissions() {
  const res = await fetch('../Assets/Resources/Missions/missions.json');
  const data = await res.json();
  state.missions = data.missions || [];
}

function applyStaticTexts() {
  document.documentElement.lang = state.lang;
  document.getElementById('app-title').textContent = t().title;
  document.getElementById('lang-label').textContent = t().lang;
  document.getElementById('mission-title').textContent = t().mission;
  document.getElementById('stations-title').textContent = t().stations;
  document.getElementById('stickers-title').textContent = t().stickers;
  document.getElementById('assemble-btn').textContent = t().assemble;
  document.getElementById('answers-title').textContent = t().choose;
  document.getElementById('not-enough').textContent = t().notEnough;
}

function startMission(index) {
  state.missionIndex = (index + state.missions.length) % state.missions.length;
  state.mission = state.missions[state.missionIndex];
  state.maxEnergy = state.mission.startEnergy;
  state.energy = state.mission.startEnergy;
  state.stickers.clear();
  hidePanels();
  render();
}

function hidePanels() {
  document.getElementById('answer-panel').classList.add('hidden');
  document.getElementById('result-panel').classList.add('hidden');
}

function spend(cost) {
  if (state.energy < cost) return false;
  state.energy -= cost;
  return true;
}

function performAction(actionId) {
  if (!spend(ACTION_COST)) return;
  const sticker = state.mission.stickers?.find(s => s.id === actionId);
  if (sticker) state.stickers.set(sticker.id, sticker.text);
  if (state.energy <= 0) completeMission(false, t().outOfPower);
  render();
}

function openAnswers() {
  if (!spend(ASSEMBLE_COST)) return;
  render();
  const panel = document.getElementById('answer-panel');
  const answers = document.getElementById('answers');
  answers.innerHTML = '';
  state.mission.answerOptions.forEach((option, idx) => {
    const b = document.createElement('button');
    b.textContent = option;
    b.onclick = () => selectAnswer(idx);
    answers.appendChild(b);
  });
  panel.classList.remove('hidden');
}

function hasRequired() {
  return (state.mission.requiredStickers || []).every(id => state.stickers.has(id));
}

function selectAnswer(answerIndex) {
  const success = hasRequired() && answerIndex === state.mission.correctAnswerIndex;
  completeMission(success, success ? t().correct : t().wrong);
}

function selectNotEnoughData() {
  const success = !hasRequired();
  completeMission(success, success ? t().missingGood : t().missingBad);
}

function completeMission(success, message) {
  document.getElementById('answer-panel').classList.add('hidden');
  const spent = state.maxEnergy - state.energy;
  const rank = spent <= 20 ? 'S' : spent <= 35 ? 'A' : 'B';
  const result = document.getElementById('result-panel');
  result.innerHTML = `
    <h3>${success ? t().success : t().fail}</h3>
    <p>${message}</p>
    <p>${t().spent(spent)}</p>
    <p>${t().rank(rank)}</p>
    <button id="next-mission">${t().next}</button>
    <button id="restart-mission">${t().restart}</button>
  `;
  result.classList.remove('hidden');
  document.getElementById('next-mission').onclick = () => startMission(state.missionIndex + 1);
  document.getElementById('restart-mission').onclick = () => startMission(state.missionIndex);
}

function render() {
  applyStaticTexts();
  document.getElementById('energy').textContent = t().energy(state.energy, state.maxEnergy);
  document.getElementById('mission-request').textContent = state.mission.request;

  const actions = document.getElementById('actions');
  actions.innerHTML = '';
  (state.mission.availableActions || []).forEach(actionId => {
    const b = document.createElement('button');
    b.textContent = t().actionCost(actionId);
    b.disabled = state.energy < ACTION_COST;
    b.onclick = () => performAction(actionId);
    actions.appendChild(b);
  });

  document.getElementById('assemble-btn').disabled = state.energy < ASSEMBLE_COST;

  const stickers = document.getElementById('stickers');
  stickers.innerHTML = '';
  for (const [id, text] of state.stickers.entries()) {
    const li = document.createElement('li');
    li.textContent = `${id}: ${text}`;
    stickers.appendChild(li);
  }
}

async function main() {
  await loadMissions();
  if (!state.missions.length) {
    alert(t().noMissions);
    return;
  }
  document.getElementById('assemble-btn').onclick = openAnswers;
  document.getElementById('not-enough').onclick = selectNotEnoughData;
  document.getElementById('lang-select').onchange = (e) => {
    state.lang = e.target.value;
    render();
  };
  startMission(0);
}

main();
